using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNA.Common
{

  public class CompareablePair<T1, T2> : IComparable<CompareablePair<T1, T2>>
    where T1 : IComparable<T1>
  {
    private T1 m_First;

    public T1 First
    {
      get { return m_First; }
      set { m_First = value; }
    }

    private T2 m_Second;

    public T2 Second
    {
      get { return m_Second; }
      set { m_Second = value; }
    }

    public CompareablePair() { }

    public CompareablePair(T1 first, T2 second)
    {
      First = first;
      Second = second;
    }

    public override bool Equals(object obj)
    {
      if (obj is CompareablePair<T1, T2>)
      {
        return this.CompareTo(obj as CompareablePair<T1, T2>) == 0;
      }
      return false;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    #region IComparable<Pair<T1,T2>> Members

    public int CompareTo(CompareablePair<T1, T2> other)
    {
      return First.CompareTo(other.First);
    }

    #endregion
  }

  public class FullyCompareablePair<T1,T2>:IComparable<FullyCompareablePair<T1,T2>>
    where T1:IComparable<T1> 
    where T2:IComparable<T2>
  {
    private T1 m_First;

    public T1 First
    {
      get { return m_First; }
      set { m_First = value; }
    }

    private T2 m_Second;

    public T2 Second
    {
      get { return m_Second; }
      set { m_Second = value; }
    }

    public FullyCompareablePair() { }

    public FullyCompareablePair(T1 first, T2 second)
    {
      First = first;
      Second = second;
    }

    public override bool Equals(object obj)
    {
      if (obj is FullyCompareablePair<T1, T2>)
      {
        return this.CompareTo(obj as FullyCompareablePair<T1, T2>) == 0;
      }
      return false;
    }

    public override int GetHashCode()
    {
      return First.GetHashCode();
    }

    #region IComparable<Pair<T1,T2>> Members

    public int CompareTo(FullyCompareablePair<T1, T2> other)
    {

      int result = First.CompareTo(other.First);
      if (result != 0) 
        return result;
      return Second.CompareTo(other.Second);
    }

    #endregion
  }
}
