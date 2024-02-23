namespace MacroModules.App.Types
{
    public class DropoutStack<T>
    {
        public int Capacity
        {
            get { return _capacity; }
            set
            {
                if (value != _capacity)
                {
                    ResizeStack(value);
                    _capacity = value;
                }
            }
        }
        private int _capacity;

        public int Count { get; private set; } = 0;

        public DropoutStack(int initialCapacity)
        {
            _capacity = initialCapacity;
            stack = new T[initialCapacity];
        }

        public void Push(T item)
        {
            stack[topIndex] = item;
            topIndex = (topIndex + 1) % stack.Length;
            Count = Math.Min(Count + 1, Capacity);
        }

        public T? Pop()
        {
            if (Count == 0)
            {
                return default;
            }

            topIndex = (topIndex > 0) ? topIndex - 1 : Capacity - 1;
            --Count;
            return stack[topIndex];
        }

        public T? Peek()
        {
            if (Count == 0)
            {
                return default;
            }

            return stack[(topIndex > 0) ? topIndex - 1 : Capacity - 1];
        }

        public void Clear()
        {
            Count = 0;
        }

        private T[] stack;
        private int topIndex = 0;

        private void ResizeStack(int newCapacity)
        {
            T[] newStack = new T[newCapacity];
            int newCount = 0;
            int newTopIndex = 0;

            int transferIndex = topIndex - Count;
            if (transferIndex < 0)
            {
                transferIndex += Capacity;
            }
            while (Count > 0)
            {
                newStack[newTopIndex] = stack[transferIndex];
                transferIndex = (transferIndex + 1) % Capacity;
                --Count;
                newTopIndex = (newTopIndex + 1) % newStack.Length;
                newCount = Math.Min(newCount + 1, newCapacity);
            }

            stack = newStack;
            Count = newCount;
            topIndex = newTopIndex;
        }
    }
}
