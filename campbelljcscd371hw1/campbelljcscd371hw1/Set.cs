/*
 * Author: Jason Campbell
 * Assignment 1: Basic C# Class Components - Set
 * 
 * since System.Collections is missing set, a common data structure, a class has been
 * created to ensure the set is a collection of unique values along with appropriate
 * methods 
 * 
 * Complete documentation found below
*/

using System;
using System.Collections;

namespace campbelljcscd371hw1
{
    /// <summary> 
    ///  is an unordered collection of values, where each value occurs 
    ///  at most once. A group of elements with three properties: 
    ///  (1) all elements belong to a universe, 
    ///  (2) either each element is a member of the set or it is not, and 
    ///  (3) the elements are unordered
    ///  <remark>implements ICollection and IEnumberable</remark>
    /// </summary>
    public class Set : ICollection, IEnumerable
    {

        private readonly ArrayList list;

        /// <summary>
        /// constructs an empty set
        /// </summary>
        public Set()
        {
            this.list = new ArrayList();
        }

        /// <summary>
        /// Returns true if the set contains no element, else false (read-only)
        /// </summary>
        public bool Empty
        {
            get
            {
                return list.Count == 0;
            }
        }

        /// <summary>
        /// Returns the number of elements in the set (read-only)
        /// </summary>
        public int Count
        {
            get
            {
                return list.Count;
            }
        }
        
        /// <summary>
        /// An object that can be used to synchronize access to the ArrayList
        /// </summary>
        /// <remarks> defaulted to "public object SyncRoot => throw new NotImplementedException();"</remarks>
        public object SyncRoot => ((ICollection)this.list).SyncRoot;

        /// <summary>
        /// True if access to the ArrayList is synchronized (thread safe); otherwise, false. The default is false.
        /// </summary>
        /// <remarks>defaulted to "public bool IsSynchronized => throw new NotImplementedException();"</remarks>
        public bool IsSynchronized => ((ICollection)this.list).IsSynchronized;

        /// <summary>
        /// Verifies that the index is valid, and finds the object at the index
        /// </summary>
        /// <param name="index">int value used as an index</param>
        /// <returns>The object at the index</returns>
        public object this[int index]
        {
            get
            {
                if (index >= 0 && index < list.Count)
                {
                    return list.IndexOf(index);
                }
                else
                {
                    throw new IndexOutOfRangeException("invalid index");
                }
            }
        }

        /// <summary>
        /// Verifies if the object is in the current list
        /// </summary>
        /// <param name="o">The object passed for verification</param>
        /// <returns>Returns a bool, true if the Set contains the object, else false</returns>
        public bool Contains(object o)
        {
            if (o == null)
            {
                throw new Exception("object cannot be null");
            }// end precondition

            if (this.list.Contains(o))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds the object o to the Set if it is not contained already
        /// </summary>
        /// <param name="o">the object passed</param>
        /// <returns>Returns a bool, true if o was added, else false</returns>
        public bool Add(object o)
        {
            if (o == null)
            {
                throw new Exception("object cannot be null");
            }// end precondition

            if (this.list.Contains(o))
            {
                return false;
            }

            this.list.Add(o);
            return true;
        }

        /// <summary>
        /// Removes the first occurrence of a specific object
        /// </summary>
        /// <param name="o"></param>
        /// <returns>Returns a bool, true if removed, else false</returns>
        public bool Remove(object o)
        {
            if (o == null)
            {
                throw new Exception("object cannot be null");
            }// end precondition

            if (this.list.Contains(o))
            {
                this.list.Remove(o);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object
        /// </summary>
        /// <param name="o">the other list</param>
        /// <returns>Returns true if and only if two Set objects store the same values</returns>
        public override bool Equals(object o)
        {
            if (o == null || !(o is Set that))
            {
                return false;
            } 
            else
            {
                if (this.list.Count == that.list.Count)
                {
                    for (int ix = 0; ix < this.list.Count; ix++)
                    {
                        if (!this.list.Contains(that.list[ix])) {
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        ///  Traverses the elements of the Set and sum the values returned 
        ///  by calling GetHashCode() on each element
        /// </summary>
        /// <returns>Returns an int value that is a representation of the Set</returns>
        public override int GetHashCode()
        {
            int total = 0;
            for (int ix = 0; ix < this.list.Count; ix++)
            {
                total += this.list[ix].GetHashCode();
            }
            return total;
        }

        /// <summary>
        /// the following format of the string that represents the current object:
        /// comma-separated list enclosed in square-brackets
        /// <example>
        ///     [ value1, value2, value3, ]
        /// </example>
        /// </summary>
        /// <returns>Returns a string representation of the Set class</returns>
        public override string ToString()
        {
            string str = "[ ";
            for (int ix = 0; ix < this.list.Count; ix++)
            {
                str += this.list[ix];
                if (ix < this.list.Count - 1)
                {
                    str += ", ";
                }
            }
            str += " ]";
            return str;
        }

        /// <summary>
        /// Copies the entire ArrayList to a compatible one-dimensional Array, starting 
        /// at the specified index of the target array
        /// </summary>
        /// <param name="array">the array</param>
        /// <param name="index">the specified index</param>
        public void CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array cannot be null");
            }
            else if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index cannot be less than zero");
            }

            this.list.CopyTo(array, index);
        }

        /// <summary>
        /// Returns an enumerator for the entire Set
        /// </summary>
        /// <returns>Returns an enumerator</returns>
        public IEnumerator GetEnumerator()
        {
            return this.list.GetEnumerator();
        }
    }
}
