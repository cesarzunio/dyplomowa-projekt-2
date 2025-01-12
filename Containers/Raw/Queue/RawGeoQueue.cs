using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Ces.Collections
{
    [NoAlias]
    public unsafe struct RawGeoQueue
    {
        const int NONE = -1;

        [NativeDisableUnsafePtrRestriction, NoAlias]
        uint* _nodes;

        [NativeDisableUnsafePtrRestriction, NoAlias]
        int* _nodeToIndexInHeap;

        [NativeDisableUnsafePtrRestriction, NoAlias]
        double* _nodeToCost;

        [NativeDisableUnsafePtrRestriction, NoAlias]
        int* _nodeToParentNode;

        [NativeDisableUnsafePtrRestriction, NoAlias]
        bool* _nodeToClosed;

        RawGeoQueueHeuristic _queueHeuristic;
        RawSpan<double2> _nodesColumnsGeoCoordRows;

        int _count;
        readonly int _capacity;
        readonly Allocator _allocator;

        public readonly int Count => _count;
        public readonly bool IsCreated => _nodes != null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RawGeoQueue(int capacity, Allocator allocator)
        {
            if (capacity <= 0)
                throw new Exception($"RawGeoQueue :: Capacity ({capacity}) must be higher than 0!");

            if (allocator == Allocator.Invalid || allocator == Allocator.None)
                throw new Exception($"RawGeoQueue :: Wrong allocator ({(int)allocator})!");

            _nodes = CesMemoryUtility.Allocate<uint>(capacity, allocator);
            _nodeToIndexInHeap = CesMemoryUtility.Allocate<int>(capacity, allocator);
            _nodeToCost = CesMemoryUtility.Allocate<double>(capacity, allocator);
            _nodeToParentNode = CesMemoryUtility.Allocate<int>(capacity, allocator);
            _nodeToClosed = CesMemoryUtility.Allocate<bool>(capacity, allocator);

            _queueHeuristic = RawGeoQueueHeuristic.Null();
            _nodesColumnsGeoCoordRows = RawSpan<double2>.Null;

            _count = 0;
            _capacity = capacity;
            _allocator = allocator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            CesMemoryUtility.FreeAndNullify(ref _nodes, _allocator);
            CesMemoryUtility.FreeAndNullify(ref _nodeToIndexInHeap, _allocator);
            CesMemoryUtility.FreeAndNullify(ref _nodeToCost, _allocator);
            CesMemoryUtility.FreeAndNullify(ref _nodeToParentNode, _allocator);
            CesMemoryUtility.FreeAndNullify(ref _nodeToClosed, _allocator);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Initialize(RawGeoQueueHeuristic queueHeuristic, RawSpan<double2> nodesColumnsGeoCoordRows)
        {
            CesMemoryUtility.MemSet(_nodeToIndexInHeap, NONE, _capacity);
            CesMemoryUtility.MemSet(_nodeToParentNode, NONE, _capacity);
            UnsafeUtility.MemClear(_nodeToClosed, UnsafeUtility.SizeOf<bool>() * _capacity);

            _queueHeuristic = queueHeuristic;
            _nodesColumnsGeoCoordRows = nodesColumnsGeoCoordRows;

            _count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(uint item, double cost)
        {
#if CES_COLLECTIONS_CHECK
            if (CesCollectionsUtility.IsOutOfRange(item, _capacity))
                throw new Exception($"RawGeoQueue :: Contains :: Node ({item}) out of range ({_capacity})!");

            if (_count == _capacity)
                throw new Exception($"RawGeoQueue :: Queue is full ({_capacity})!");
#endif

            _nodes[_count++] = item;

            _nodeToIndexInHeap[item] = _count - 1;
            _nodeToCost[item] = cost;
            _nodeToParentNode[item] = NONE;
            _nodeToClosed[item] = false;

            HeapifyUp(_count - 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddOrUpdate(uint item, double cost)
        {
#if CES_COLLECTIONS_CHECK
            if (CesCollectionsUtility.IsOutOfRange(item, _capacity))
                throw new Exception($"RawGeoQueue :: Contains :: Node ({item}) out of range ({_capacity})!");
#endif

            int index = _nodeToIndexInHeap[item];

            if (index != NONE) // node is present, update cost
            {
                _nodeToCost[item] = cost;
                HeapifyUp(index);
            }
            else
            {
                Add(item, cost);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint Pop()
        {
#if CES_COLLECTIONS_CHECK
            if (_count == 0)
                throw new Exception($"RawGeoQueue :: Queue is empty ({_capacity})!");
#endif

            uint root = _nodes[0];

            _nodes[0] = _nodes[_count - 1];
            _nodeToIndexInHeap[_nodes[0]] = 0;

            _count--;
            _nodeToIndexInHeap[root] = NONE;

            HeapifyDown(0);

            return root;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryPop(out uint item)
        {
            if (Count == 0)
            {
                item = default;
                return false;
            }

            item = Pop();
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Contains(uint item)
        {
#if CES_COLLECTIONS_CHECK
            if (CesCollectionsUtility.IsOutOfRange(item, _capacity))
                throw new Exception($"RawGeoQueue :: Contains :: Node ({item}) out of range ({_capacity})!");
#endif

            return _nodeToIndexInHeap[item] != NONE;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetCost(uint item)
        {
#if CES_COLLECTIONS_CHECK
            if (CesCollectionsUtility.IsOutOfRange(item, _capacity))
                throw new Exception($"RawGeoQueue :: Contains :: Node ({item}) out of range ({_capacity})!");
#endif

            return _nodeToCost[item];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetCost(uint item, out double cost)
        {
#if CES_COLLECTIONS_CHECK
            if (CesCollectionsUtility.IsOutOfRange(item, _capacity))
                throw new Exception($"RawGeoQueue :: Contains :: Node ({item}) out of range ({_capacity})!");
#endif

            cost = _nodeToCost[item];
            return Contains(item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetParent(uint item, out uint parent)
        {
#if CES_COLLECTIONS_CHECK
            if (CesCollectionsUtility.IsOutOfRange(item, _capacity))
                throw new Exception($"RawGeoQueue :: Contains :: Node ({item}) out of range ({_capacity})!");
#endif

            int parentNode = _nodeToParentNode[item];

            if (!Contains(item) || parentNode == NONE)
            {
                parent = default;
                return false;
            }

            parent = (uint)parentNode;
            return true;
        }

        void HeapifyUp(int index)
        {
            if (index < 0 || index > _count - 1)
                return;

            var item = _nodes[index];

            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;
                var parentItem = _nodes[parentIndex];

                if (IsLowerCost(item, parentItem))
                {
                    _nodeToIndexInHeap[item] = parentIndex;
                    _nodeToIndexInHeap[parentItem] = index;

                    (_nodes[index], _nodes[parentIndex]) = (_nodes[parentIndex], _nodes[index]);

                    index = parentIndex;
                }
                else
                {
                    break;
                }
            }
        }

        void HeapifyDown(int index)
        {
            int lastIndex = _count - 1;

            while (true)
            {
                int leftChildIndex = 2 * index + 1;
                int rightChildIndex = 2 * index + 2;
                int smallest = index;

                if (leftChildIndex <= lastIndex && IsLowerCost(_nodes[leftChildIndex], _nodes[smallest]))
                {
                    smallest = leftChildIndex;
                }

                if (rightChildIndex <= lastIndex && IsLowerCost(_nodes[rightChildIndex], _nodes[smallest]))
                {
                    smallest = rightChildIndex;
                }

                if (smallest == index)
                    break;

                _nodeToIndexInHeap[_nodes[index]] = smallest;
                _nodeToIndexInHeap[_nodes[smallest]] = index;

                (_nodes[index], _nodes[smallest]) = (_nodes[smallest], _nodes[index]);
                index = smallest;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool IsLowerCost(uint lhs, uint rhs)
        {
            double lhsG = _nodeToCost[lhs];
            double rhsG = _nodeToCost[rhs];

            double lhsH = _queueHeuristic.CalculateHeuristic(lhs, in _nodesColumnsGeoCoordRows);
            double rhsH = _queueHeuristic.CalculateHeuristic(rhs, in _nodesColumnsGeoCoordRows);

            double lhsF = lhsG + lhsH;
            double rhsF = rhsG + rhsH;

            bool lowerG = lhsG < rhsG;
            bool lowerF = lhsF < rhsF;
            bool equalF = math.abs(lhsF - rhsF) < math.EPSILON_DBL;

            return equalF ? lowerG : lowerF;
        }
    }
}