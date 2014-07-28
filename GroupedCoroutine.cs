using System.Collections.Generic;
using System.Linq;

namespace ICS {

	public class GroupedCoroutine : ICoroutineBase {
		LinkedList<ICoroutineBase> _list;
		
		public GroupedCoroutine()
		{
			_list = new LinkedList<ICoroutineBase>();	
		}
		
		public GroupedCoroutine(ICoroutineBase c1, ICoroutineBase c2)
		{ 
			_list = new LinkedList<ICoroutineBase>();
			_list.AddLast(c1);
			_list.AddLast(c2);
		}
		
		override public bool finished { get { return _list.Count == 0; } }
		
		override public ICoroutineBase Update()
		{
			LinkedListNode<ICoroutineBase> node = _list.First;
			while(node != null)
			{
				node.Value = node.Value.Update();
				LinkedListNode<ICoroutineBase> next = node.Next;
				if(node.Value.finished)
					_list.Remove(node);
				node = next;
			}
			
			return this;
		}
		
		public void AddToList(IEnumerable<ICoroutineBase> newList)
		{
			foreach(ICoroutineBase c in newList)
				_list.AddLast(c);
		}
		
		public static GroupedCoroutine operator +(GroupedCoroutine left, ICoroutineBase right)
		{
			if( right is GroupedCoroutine)
			{
				left.AddToList( ((GroupedCoroutine)right)._list );
			}
			else
			{
				left._list.AddLast(right);
			}
			
			return left;
		}
	}

}