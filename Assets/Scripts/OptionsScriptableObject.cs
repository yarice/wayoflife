using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WayOfLife.Model
{
    [CreateAssetMenu(fileName = "OptionsScriptableObject",menuName = "ScriptableObjects/Options")]
    public class OptionsScriptableObject : ScriptableObject
    {
        [SerializeField] [Range(0, 500)] private int speedlarge;
        [SerializeField] [Range(501, 1000)] private int speedmedium;
        [SerializeField] [Range(1001, 2000)] private int speedsmall;
        [SerializeField] [Range(0, 50)] private int sizesmall;
        [SerializeField] [Range(51, 100)] private int sizemedium;
        [SerializeField] [Range(101, 150)] private int sizelarge;
        [SerializeField] [Range(0, 33)] private int probsmall;
        [SerializeField] [Range(34, 66)] private int probmedium;
        [SerializeField] [Range(67, 100)] private int problarge;
        private Queue<int> speeds;
        private Queue<int> probs;
        private Queue<int> sizes;


        public void InitInputsQueues()
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
        public int getMaximalSize()
        {
            return sizelarge;
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