using System.Collections.Generic;
using System;

namespace LockStep {
    public static class Algorithm {
        public static void Swap<T>(List<T> data, int a, int b){
            var temp = data[a];
            data[a] = data[b];
            data[b] = temp;
        }

        public static T Median<T>(T a, T b, T c, Func<T, T, int> __comp){
            if (__comp(a, b) < 0) {
                if (__comp(b, c) < 0)
                    return b;
                else {
                    if (__comp(a, c) < 0)
                        return c;
                    else
                        return a;
                }
            }
            else {
                //a>b
                if (__comp(a, c) < 0)
                    return a;
                else {
                    if (__comp(b, c) < 0)
                        return c;
                    else
                        return b;
                }
            }
        }

        public static T NthElement<T>(List<T> data, int __first, int __nth, int __last, Func<T, T, int> __comp){
            while (__last - __first > 3) {
                var _cut = Partition(data, __first, __last, __comp);
                if (_cut == __nth)
                    return data[_cut];
                else if (_cut > __nth)
                    __first = _cut;
                else
                    __last = _cut;
            }
            /**/


            //__insertion_sort
            var n = __last - __first;
            for (int i = __first + 1; i < __last; ++i) {
                var x = data[i];
                int j = i - 1;
                while (__comp(data[j], x) > 0 && j >= __first) {
                    data[j + 1] = data[j];
                    j--;
                }

                data[j + 1] = x;
            }

            return data[__nth];
        }

        //partition函数
        public static int Partition<T>(List<T> data, int __first, int __last, Func<T, T, int> __comp){
            var __pivot = Median(data[__first], data[__last], data[(__first + __last) / 2], __comp);
            while (true) {
                while (__comp(data[__first], __pivot) < 0)
                    ++__first;
                --__last;
                while (__comp(__pivot, data[__last]) < 0)
                    --__last;
                if (!(__first < __last))
                    return __first;
                //swap 
                Swap(data, __first, __last);
                ++__first;
            }
        }

        public static int Partition<T>(List<T> data, int __first, int __last, Func<T, bool> __comp){
            var _endIdx = __last - 1;
            var _beginIdx = __first;
            while (true) {
                while (__first <= _endIdx && __comp(data[__first]))
                    ++__first;
                --__last;
                while (__last >= _beginIdx && __comp(data[__last]))
                    --__last;
                if (!(__first < __last))
                    return __first;
                //swap 
                Swap(data, __first, __last);
                ++__first;
            }
        }
    }
}