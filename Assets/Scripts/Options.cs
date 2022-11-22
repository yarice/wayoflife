using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WayOfLife.Model
{
    public class Options : ScriptableObject
    {
        private Queue<int> speeds;
        private Queue<int> probs;
        private Queue<int> sizes;


        public void InitInputsQueues(int speedlarge,int speedmedium,int speedsmall,int sizelarge,int sizemedium,int sizesmall,int problarge,int probmedium,int probsmall)
        {
            speeds = new Queue<int>();
            sizes = new Queue<int>();
            probs = new Queue<int>();
            speeds.Enqueue(speedlarge);
            speeds.Enqueue(speedmedium);
            speeds.Enqueue(speedsmall);
            sizes.Enqueue(sizelarge);
            sizes.Enqueue(sizemedium);
            sizes.Enqueue(sizesmall);
            probs.Enqueue(problarge);
            probs.Enqueue(probmedium);
            probs.Enqueue(probsmall);
        }
        

        private Queue<int> getQueue(ValuesEnum value)
        {
            switch (value)
            {
                case ValuesEnum.Prob:
                    return probs;
                case ValuesEnum.Size:
                    return sizes;

                default:
                    return speeds;
            }
        }

        public int GetCurrentValue(ValuesEnum value)
        {
            return getQueue(value).Peek();
        }

        public int GetNextValue(ValuesEnum value)
        {
            Queue<int> queue = getQueue(value);
            int temp = queue.Dequeue();
            queue.Enqueue(temp);
            return temp;
        }
    }
}