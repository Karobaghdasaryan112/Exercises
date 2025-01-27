namespace CustomLinkedList.Services
{
    public class CustomLinkedListNode<T>
    {
        public T? value { get; set; }

        public CustomLinkedListNode<T>? _next;

        public CustomLinkedListNode<T>? _prev;

        public CustomLinkedListNode(T? value, CustomLinkedListNode<T>? next, CustomLinkedListNode<T>? prev)
        {
            this.value = value;
            _next = next;
            _prev = prev;
        }

        public CustomLinkedListNode(T? value) 
        {
            this.value = value;
        }

        public CustomLinkedListNode(CustomLinkedListNode<T> next,CustomLinkedListNode<T> prev)
        {
            _next = next;
            _prev = prev;
        }


        public CustomLinkedListNode() 
        {

        }
    }
}