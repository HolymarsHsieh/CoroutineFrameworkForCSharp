using UnityEngine;
using System.Collections;

namespace ICS.Predefined {		
	public class WaitSecondsCoroutine : ICoroutineBase {
		float _targetTime;
		
		public WaitSecondsCoroutine( float waitTime ) {
			_targetTime = Time.time + waitTime;
		}
		
		override public bool finished {
			get { return Time.time >= _targetTime; }
		}
		
		override public ICoroutineBase Update() { return this; }
	}

	public class LogicCoroutine : ICoroutineBase
	{
		IEnumerator _block;
		bool _finished = false;
			
		public LogicCoroutine(IEnumerator b) { _block = b; }

		ICoroutineBase covertToCoroutine(object o) {
			IEnumerator b = o as IEnumerator;
			if( b != null )
				return new LogicCoroutine(b);
			else 
				return o as ICoroutineBase;
		}

		override public bool finished {get { return _finished; } }

		override public ICoroutineBase Update()
		{
			if(_finished) return this;

			if( _block.MoveNext() )
			{
				ICoroutineBase waitFor = covertToCoroutine(_block.Current);
				if( waitFor != null )
				{
	                waitFor = waitFor.Update();
	                if(waitFor.finished)
	                    return this.Update();
	                else
					    return new OrderedCoroutine(waitFor, this);
				}   
			}
			else
				_finished = true;
			
			return this;
		}

		~LogicCoroutine()
		{
			((System.IDisposable)_block).Dispose();
			_block = null;
		}
	}
}
