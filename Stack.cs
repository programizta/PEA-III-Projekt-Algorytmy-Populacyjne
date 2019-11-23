using System;

namespace III_Projekt
{
    class Stack
    {
        public int StackSize { get; private set; }
        private int[] numbersOnStack;

        public Stack()
        {
            StackSize = 0;
        }

        public void Push(int value)
        {
            int auxStackSize = StackSize + 1;
            int[] auxNumbers = new int[auxStackSize];

            auxNumbers[0] = value;

            for (int i = 1; i < auxStackSize; i++)
            {
                auxNumbers[i] = numbersOnStack[i - 1];
            }

            StackSize = auxStackSize;
            numbersOnStack = auxNumbers;
        }

        public void Pop()
        {
            int auxStackSize = StackSize - 1;
            int[] auxNumbers = new int[auxStackSize];

            for (int i = 0; i < auxStackSize; i++)
            {
                auxNumbers[i] = numbersOnStack[i + 1];
            }

            StackSize = auxStackSize;
            numbersOnStack = auxNumbers;
        }

        public void Display()
        {
            for (int i = StackSize - 1; i >= 0; i--)
            {
                if (i > 0) Console.Write(numbersOnStack[i] + " -> ");
                else Console.Write(numbersOnStack[i]);
            }
        }

        public void CopyFrom(Stack stack)
        {
            for (int i = stack.StackSize - 1; i >= 0; i--)
            {
                Push(stack.numbersOnStack[i]);
            }
        }

        public void Clear()
        {
            while (StackSize > 0)
            {
                Pop();
            }
        }

        public int GetElement(int index)
        {
            return numbersOnStack[index];
        }
    }
}