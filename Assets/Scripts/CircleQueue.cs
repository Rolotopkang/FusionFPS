using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleQueue<T>
{
     /// <summary> 
        /// 队列数组 
        /// </summary> 
        private T[] _queue; 
        /// <summary> 
        /// 队首索引 
        /// </summary> 
        private int _front; 
        /// <summary> 
        /// 队尾索引 
        /// </summary> 
        private int _rear; 
           
        /// <summary> 
        /// 队列的内存大小，但实际可用大小为_capacity-1 
        /// </summary> 
        private int _capacity; 
   
        public CircleQueue(int queueSize)
        { 
            if (queueSize < 1) 
                throw new IndexOutOfRangeException("传入的队列长度不能小于1。"); 
   
            //设置队列容量 
            _capacity = queueSize; 
               
            //创建队列数组 
            _queue = new T[queueSize]; 
   
            //初始化队首和队尾索引 
            _front = _rear = 0; 
        } 
   
        /// <summary> 
        /// 添加一个元素 
        /// </summary> 
        /// <param name="item"></param> 
        public void Push(T item) 
        { 
            //队列已满 
            if (GetNextRearIndex() == _front) 
            { 
                //扩大数组 
                T[] newQueue = new T[2 * _capacity]; 
                   
                if (newQueue == null) 
                    throw new ArgumentOutOfRangeException("数据容量过大，超出系统内存大小。"); 
                //队列索引尚未回绕 
                if (_front == 0) 
                { 
                    //将旧队列数组数据转移到新队列数组中 
                    Array.Copy(_queue, newQueue, _capacity); 
                } 
                else 
                { 
                    //如果队列回绕，刚需拷贝再次， 
                    //第一次将队首至旧队列数组最大长度的数据拷贝到新队列数组中 
                    Array.Copy(_queue, _front, newQueue, _front, _capacity - _rear - 1); 
                    //第二次将旧队列数组起始位置至队尾的数据拷贝到新队列数组中 
                    Array.Copy(_queue, 0, newQueue, _capacity, _rear + 1); 
                    //将队尾索引改为新队列数组的索引 
                    _rear = _capacity + 1; 
                } 
   
                _queue = newQueue; 
                _capacity *= 2; 
            } 
   
            //累加队尾索引，并添加当前项 
            _rear = GetNextRearIndex(); 
            _queue[_rear] = item; 
        } 
   
        /// <summary> 
        /// 获取队首元素 
        /// </summary> 
        /// <returns></returns> 
        public T FrontItem() 
        { 
            if (IsEmpty()) 
                throw new ArgumentOutOfRangeException("队列为空。"); 
               
            return _queue[GetNextFrontIndex()]; 
        } 
   
        /// <summary> 
        /// 获取队尾元素 
        /// </summary> 
        /// <returns></returns> 
        public T RearItem() 
        { 
            if (IsEmpty()) 
                throw new ArgumentOutOfRangeException("队列为空。"); 
   
            return _queue[_rear]; 
        } 
   
        /// <summary> 
        /// 弹出一个元素 
        /// </summary> 
        /// <returns></returns> 
        public T Pop() 
        { 
            if (IsEmpty()) 
                throw new ArgumentOutOfRangeException("队列为空。"); 
   
            _front = GetNextFrontIndex(); 
            return _queue[_front]; 
        } 
   
        /// <summary> 
        /// 队列是否为空 
        /// </summary> 
        /// <returns></returns> 
        public bool IsEmpty() 
        { 
            return _front == _rear; 
        } 
        /// <summary> 
        /// 获取下一个索引 
        /// </summary> 
        /// <returns></returns> 
        private int GetNextRearIndex() 
        { 
            if (_rear + 1 == _capacity) 
            { 
                return 0; 
            } 
            return _rear + 1; 
        } 
   
        /// <summary> 
        /// 获取下一个索引 
        /// </summary> 
        /// <returns></returns> 
        private int GetNextFrontIndex() 
        { 
            if (_front + 1 == _capacity) 
            { 
                return 0; 
            } 
            return _front + 1; 
        } 
}
