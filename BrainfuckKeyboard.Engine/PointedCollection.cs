namespace BrainfuckKeyboard.Engine
{
    public class PointedCollection<T>
    {
        private LinkedList<T?> items = new();
        private LinkedListNode<T?> pointedNode;

        public int PointedIndex { get; private set; }
        public T? PointedItem
        {
            get => pointedNode.Value;
            set => pointedNode.Value = value;
        }
        public int AllocatedSize => items.Count;

        public PointedCollection() => pointedNode = items.AddFirst(default(T));

        public void MoveForward()
        {
            pointedNode = pointedNode.Next ?? items.AddLast(default(T));
            PointedIndex++;
        }
        public void MoveBack()
        {
            if (PointedIndex == 0)
                throw new InvalidOperationException("Index is at the beginning of the collection.");

            pointedNode = pointedNode.Previous ?? items.AddFirst(default(T));
            PointedIndex--;
        }
    }
}
