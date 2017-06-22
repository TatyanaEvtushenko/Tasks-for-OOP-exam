using System;
using System.Collections.Generic;
using System.ComponentModel;
using OOP_ForExam.Comparers;
using OOP_ForExam.Tasks;

namespace OOP_ForExam
{
    static class Worker
    {
        private static void ShowAction(string action, System.Collections.IEnumerable collection)
        {
            Console.WriteLine(action);
            foreach (var item in collection)
            {
                Console.Write($" {item}");
            }
            Console.WriteLine("\n");
        }

        public static void ShowBitArray()
        {
            var bitArray = new BitArray(new bool[] {true, false, true, true, false});
            Action<string> showInfo = (action) => {
                Console.WriteLine($"Count = {bitArray.Count}, CountOfReservedElements = {bitArray.CountOfReservedElements}");
                ShowAction(action, bitArray);
            };
            showInfo("Create");
            bitArray.Add(true);
            showInfo("Add true");
            bitArray.Add(false);
            showInfo("Add false");
            bitArray.Add(false);
            showInfo("Add false");
            bitArray.Add(true);
            showInfo("Add true");
            bitArray.IncreaseArray(20);
            showInfo("Increase (20)");
            bitArray[2] = false;
            showInfo("set bitArray[2] = false");
            bitArray[18] = true;
            showInfo("set bitArray[18] = true");
            bitArray[9] = true;
            showInfo("set bitArray[9] = true");
            bitArray[0] = false;
            showInfo("set bitArray[0] = false");
            showInfo($"get bitArray[2] = {bitArray[2]}");
            showInfo($"get bitArray[14] = {bitArray[14]}");
            showInfo($"get bitArray[5] = {bitArray[5]}");
        }

        public static void ShowCircularBuffer()
        {
            var buffer = new CircularBuffer(5);
            Action<string> showInfo = (action) => {
                Console.WriteLine($"IndexToRead = {buffer.IndexToRead}, IndexToWrite = {buffer.IndexToWrite}, CountOfData = {buffer.CountOfData}");
                ShowAction(action, buffer);
            };
            buffer.Write(1);
            showInfo("WRITE 1");
            buffer.Write(2);
            showInfo("WRITE 2");
            buffer.Write(3);
            showInfo("WRITE 3");
            buffer.Write(4);
            showInfo("WRITE 4");
            buffer.Write(5);
            showInfo("WRITE 5");
            try
            {
                buffer.Write(6);
            }
            catch (Exception e)
            {
                showInfo($"TRY WRITE 6 - {e.Message}");
            }
            showInfo($"READ - {buffer.Read()}");
            showInfo($"READ - {buffer.Read()}");
            showInfo($"READ - {buffer.Read()}");
            buffer.Write(6);
            showInfo("WRITE 6");
            buffer.IncreaseBuffer(8);
            showInfo("INCREASE TO 8 ELEMENTS");
            buffer.Write(111);
            showInfo("WRITE 111");
            buffer.Write(123);
            showInfo("WRITE 123");
            buffer.Write(199);
            showInfo("WRITE 199");
            buffer.Write(255);
            showInfo("WRITE 255");
        }

        public static void ShowCircularBufferTemplate()
        {
            var buffer = new CircularBuffer<string>(5);
            Action<string> showInfo = (action) => {
                Console.WriteLine($"IndexToRead = {buffer.IndexToRead}, IndexToWrite = {buffer.IndexToWrite}, CountOfData = {buffer.CountOfData}");
                ShowAction(action, buffer);
            };
            buffer.Write("one");
            showInfo("WRITE one");
            buffer.Write("two");
            showInfo("WRITE two");
            buffer.Write("three");
            showInfo("WRITE three");
            buffer.Write("four");
            showInfo("WRITE four");
            buffer.Write("five");
            showInfo("WRITE five");
            try
            {
                buffer.Write("six");
            }
            catch (Exception e)
            {
                showInfo($"TRY WRITE six - {e.Message}");
            }
            showInfo($"READ - {buffer.Read()}");
            showInfo($"READ - {buffer.Read()}");
            showInfo($"READ - {buffer.Read()}");
            buffer.Write("six");
            showInfo("WRITE six");
            buffer.IncreaseBuffer(8);
            showInfo("INCREASE 8");
            buffer.Write("qqq");
            showInfo("WRITE qqq");
            buffer.Write("www");
            showInfo("WRITE www");
            buffer.Write("eee");
            showInfo("WRITE eee");
            buffer.Write("end");
            showInfo("WRITE end");
        }

        public static void ShowCollection()
        {
            var collection = new Collection(2);
            CollectionChangeEventHandler showInfo = (sender, e) =>
            {
                Console.WriteLine($"Count = {collection.Count}, CountOfReservedElements = {collection.CountOfReservedElements}");
                ShowAction($"{e.Action}: {e.Element}", collection);
            };
            collection.InsertEvent += showInfo;
            collection.RemoveEvent += showInfo;
            collection.ClearEvent += showInfo;
            collection.Insert(1, new CollectionItem(collection, "first"));
            collection.Insert(1, new CollectionItem(collection, 1.2));
            collection.Insert(1, new CollectionItem(collection, new Stack()));
            collection.RemoveAt(1);
            collection.Clear();
            collection.Insert(1, new CollectionItem(collection, "aaaaaaaaaaaa"));
            collection.Insert(0, new CollectionItem(collection, 1234567893456789));
            var otherCollection = new Collection();
            collection.CopyTo(otherCollection);
            ShowAction("Copy to", otherCollection);
            var str = new CollectionItem(otherCollection, "qwertyuio");
            collection.Insert(1, str);
            collection.Sort();
            ShowAction("Sort", collection);
            ShowAction($"Search by item = {str}: index - {collection.IndexOf(str)}", collection);
        }

        public static void ShowCollectionTemplate()
        {
            var collection = new Collection<string>();
            CollectionChangeEventHandler showInfo = (sender, e) =>
            {
                Console.WriteLine($"Count = {collection.Count}, CountOfReservedElements = {collection.CountOfReservedElements}");
                ShowAction($"{e.Action}: {e.Element}", collection);
            };
            collection.InsertEvent += showInfo;
            collection.RemoveEvent += showInfo;
            collection.ClearEvent += showInfo;
            collection.Insert(1, new CollectionItem<string>(collection, "first"));
            collection.Insert(1, new CollectionItem<string>(collection, "second"));
            collection.Insert(1, new CollectionItem<string>(collection, "third"));
            collection.RemoveAt(1);
            collection.Clear();
            collection.Insert(1, new CollectionItem<string>(collection, "aaaaaaaaaaaa"));
            collection.Insert(0, new CollectionItem<string>(collection, "bbbbbbbbbbbbb"));
            var otherCollection = new Collection<string>();
            collection.CopyTo(otherCollection);
            ShowAction("Copy to", otherCollection);
            var str = new CollectionItem<string>(otherCollection, "qwertyuio");
            collection.Insert(1, str);
            collection.Sort();
            ShowAction("Sort", collection);
            ShowAction($"Search by item = {str}: index - {collection.IndexOf(str)}", collection);
        }

        public static void ShowDictionary()
        {
            var dictionary = new Dictionary();
            CollectionChangeEventHandler showInfo = (sender, e) =>
            {
                Console.WriteLine($"Count = {dictionary.Count}, IsSorted = {dictionary.IsSorted}");
                ShowAction($"{e.Action}: {e.Element}", dictionary);
            };
            dictionary.InsertEvent += showInfo;
            dictionary.RemoveEvent += showInfo;
            dictionary.ClearEvent += showInfo;
            dictionary.Add("stack", new Stack());
            dictionary.Add("list", new List());
            dictionary.Add("bit array", new BitArray());
            dictionary.Add(new KeyValuePair<string, object>("linked list", new LinkedList()));
            dictionary.Remove("list");
            dictionary.Add(new KeyValuePair<string, object>("collection", new Collection()));
            dictionary.SortByKey();
            Console.WriteLine($"Count = {dictionary.Count}, IsSorted = {dictionary.IsSorted}");
            ShowAction($"Sort key: ", dictionary);
            dictionary.Add(new KeyValuePair<string, object>("matrix", new Matrix(3, 5)));
            dictionary.SortByValue();
            Console.WriteLine($"Count = {dictionary.Count}, IsSorted = {dictionary.IsSorted}");
            ShowAction($"Sort value: ", dictionary);
            dictionary.Remove("collection");
            dictionary.Clear();
            dictionary.Add("dictionary", new Dictionary());
            dictionary.Add("list", new List());
            Console.WriteLine($"Count = {dictionary.Count}, IsSorted = {dictionary.IsSorted}");
            Console.WriteLine(dictionary["dictionary"]);
            Console.WriteLine(dictionary["list"]);
        }

        public static void ShowDictionaryTemplate()
        {
            var dictionary = new Tasks.Dictionary<int, string>();
            CollectionChangeEventHandler showInfo = (sender, e) =>
            {
                Console.WriteLine($"Count = {dictionary.Count}, IsSorted = {dictionary.IsSorted}");
                ShowAction($"{e.Action}: {e.Element}", dictionary);
            };
            dictionary.InsertEvent += showInfo;
            dictionary.RemoveEvent += showInfo;
            dictionary.ClearEvent += showInfo;
            dictionary.Add(4, "четыре");
            dictionary.Add(100, "сто");
            dictionary.Add(27, "27");
            dictionary.Add(new KeyValuePair<int, string>(-9, "минус девять"));
            dictionary.Remove(27);
            dictionary.Add(new KeyValuePair<int, string>(0, "nul"));
            dictionary.SortByKey();
            Console.WriteLine($"Count = {dictionary.Count}, IsSorted = {dictionary.IsSorted}");
            ShowAction($"Sort key: ", dictionary);
            dictionary.Add(new KeyValuePair<int, string>(1234, "mnogo"));
            dictionary.SortByValue();
            Console.WriteLine($"Count = {dictionary.Count}, IsSorted = {dictionary.IsSorted}");
            ShowAction($"Sort value: ", dictionary);
            dictionary.Remove(100);
            dictionary.Clear();
            dictionary.Add(2, "fhk");
            dictionary.Add(78, "qqqqqqqqqqqqq");
            Console.WriteLine($"Count = {dictionary.Count}, IsSorted = {dictionary.IsSorted}");
            Console.WriteLine(dictionary[2]);
            Console.WriteLine(dictionary[78]);
        }

        public static void ShowDictionaryTemplateWithStringKey()
        {
            var dictionary = new Dictionary<double>();
            CollectionChangeEventHandler showInfo = (sender, e) =>
            {
                Console.WriteLine($"Count = {dictionary.Count}, IsSorted = {dictionary.IsSorted}");
                ShowAction($"{e.Action}: {e.Element}", dictionary);
            };
            dictionary.InsertEvent += showInfo;
            dictionary.RemoveEvent += showInfo;
            dictionary.ClearEvent += showInfo;
            dictionary.Add("четыре", 4.00);
            dictionary.Add("сто", 100.0);
            dictionary.Add("27", 27.11);
            dictionary.Add(new KeyValuePair<string, double>("минус девять", -9.000));
            dictionary.Remove("27");
            dictionary.Add(new KeyValuePair<string, double>("nul", 0));
            dictionary.SortByKey();
            Console.WriteLine($"Count = {dictionary.Count}, IsSorted = {dictionary.IsSorted}");
            ShowAction($"Sort key: ", dictionary);
            dictionary.Add(new KeyValuePair<string, double>("mnogo", 1234567.8765432));
            dictionary.SortByValue();
            Console.WriteLine($"Count = {dictionary.Count}, IsSorted = {dictionary.IsSorted}");
            ShowAction($"Sort value: ", dictionary);
            dictionary.Remove("сто");
            dictionary.Clear();
            dictionary.Add("fhk", 2232.3);
            dictionary.Add("qqqqqqqqqqqqq", -3);
            Console.WriteLine($"Count = {dictionary.Count}, IsSorted = {dictionary.IsSorted}");
            Console.WriteLine(dictionary["qqqqqqqqqqqqq"]);
            Console.WriteLine(dictionary["fhk"]);
        }

        public static void ShowLinkedList()
        {
            var list = new LinkedList();
            CollectionChangeEventHandler showInfo = (sender, e) =>
            {
                Console.WriteLine($"First = {list.First}, Next = {list.Next}");
                ShowAction($"{e.Action}: {e.Element}", list);
            };
            list.InsertEvent += showInfo;
            list.RemoveEvent += showInfo;
            list.ClearEvent += showInfo;
            list.AddLast("hello");
            list.AddLast(123);
            var name = "Any, ";
            list.AddFirst(name);
            list.AddLast(new Stack());
            var str = " you";
            list.AddLast(str);
            list.AddLast(-12.343);
            list.Replace(str, new ObjectComparer());
            Console.WriteLine($"First = {list.First}, Next = {list.Next}");
            ShowAction($"Replace: {str}", list);
            list.Remove(name);
            list.Clear();
            list.AddFirst("one");
            list.AddFirst(2.0);
            list.AddLast(3);
            var otherList = new Tasks.LinkedList();
            otherList.InsertEvent += (sender, e) =>
            {
                Console.WriteLine($"First = {otherList.First}, Next = {otherList.Next}");
                ShowAction($"{e.Action}: {e.Element}", otherList);
            };
            otherList.AddLast("sdfgtyhuiop");
            list.CopyTo(otherList);
            ShowAction("Copy: ", otherList);
        }

        public static void ShowLinkedListTemplate()
        {
            var list = new Tasks.LinkedList<string>();
            CollectionChangeEventHandler showInfo = (sender, e) =>
            {
                Console.WriteLine($"First = {list.First}, Next = {list.Next}");
                ShowAction($"{e.Action}: {e.Element}", list);
            };
            list.InsertEvent += showInfo;
            list.RemoveEvent += showInfo;
            list.ClearEvent += showInfo;
            list.AddLast("hello");
            list.AddLast(", what");
            var name = "Any, ";
            list.AddFirst(name);
            list.AddLast(" are");
            var str = " you";
            list.AddLast(str);
            list.AddLast(" doing?");
            list.Replace(str, "they");
            Console.WriteLine($"First = {list.First}, Next = {list.Next}");
            ShowAction($"Replace: {str}", list);
            list.Remove(name);
            list.Clear();
            list.AddFirst("one");
            list.AddFirst("two");
            list.AddLast("3");
            var otherList = new Tasks.LinkedList<string>();
            otherList.InsertEvent += (sender, e) =>
            {
                Console.WriteLine($"First = {otherList.First}, Next = {otherList.Next}");
                ShowAction($"{e.Action}: {e.Element}", otherList);
            };
            otherList.AddLast("sdfgtyhuiop");
            list.CopyTo(otherList);
            ShowAction($"Copy: ", otherList);
        }

        public static void ShowList()
        {
            var list = new List(5);
            list.Insert(1, 1.1);
            ShowAction("Insert 1,1 in 1", list);
            const string str = "hello";
            list.Insert(1, str);
            ShowAction("Insert 'hello' in 1", list);
            list.Insert(1, 9876543212345678);
            ShowAction("Insert 9876543212345678 in 1", list);
            list.Insert(1, 4);
            ShowAction("Insert 4 in 1", list);
            list.Insert(1, new object());
            ShowAction("Insert new object in 1", list);
            list.Insert(1, new Stack());
            ShowAction("Insert new stack in 1", list);
            list.RemoveAt(3);
            ShowAction("Remove 3", list);
            list.RemoveAt(1);
            ShowAction("Remove 1", list);
            var otherList = new List();
            list.CopyTo(otherList);
            ShowAction("Copy to other list", otherList);
            list.Sort();
            ShowAction("Sort", list);
            ShowAction($"Find index {list.IndexOf(str)} by item 5,5", list);
        }

        public static void ShowListTemplate()
        {
            var list = new Tasks.List<decimal>(5);
            list.Insert(1, (decimal)1.1);
            ShowAction("Insert 1,1 in 1", list);
            list.Insert(1, (decimal)2.2);
            ShowAction("Insert 2,2 in 1", list);
            list.Insert(1, (decimal)3.3);
            ShowAction("Insert 3,3 in 1", list);
            list.Insert(1, (decimal)4.4);
            ShowAction("Insert 4,4 in 1", list);
            list.Insert(1, (decimal)5.5);
            ShowAction("Insert 5,5 in 1", list);
            list.Insert(1, (decimal)6.6);
            ShowAction("Insert 6,6 in 1", list);
            list.RemoveAt(3);
            ShowAction("Remove 3", list);
            list.RemoveAt(1);
            ShowAction("Remove 1", list);
            var otherList = new Tasks.List<decimal>();
            list.CopyTo(otherList);
            ShowAction("Copy to other list", otherList);
            list.Sort();
            ShowAction("Sort", list);
            ShowAction($"Find index {list.IndexOf((decimal)5.5)} by item 5,5", list);
        }

        public static void ShowMatrix()
        {
            var matrix = new Matrix(3, 4);
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    matrix[i, j] = (i + j + 1) * (j + 1);
                }
            }
            Console.WriteLine("Matrix");
            Console.WriteLine(matrix);

            var otherMatrix = new Matrix(4, 3);
            for (ushort i = 0; i < 4; i++)
            {
                for (ushort j = 0; j < 3; j++)
                {
                    otherMatrix[i, j] = (i + j + 1) * (i + 1);
                }
            }
            Console.WriteLine("\nOther matrix");
            Console.WriteLine(otherMatrix);

            Console.WriteLine("\nTranspose matrix");
            Console.WriteLine(matrix.Transpose());
            Console.WriteLine("\nTranspose other matrix");
            Console.WriteLine(otherMatrix.Transpose());

            Console.WriteLine("\nMultiply matrixes");
            Console.WriteLine(matrix.Multiply(otherMatrix));
        }

        public static void ShowMatrixTemplate()
        {
            var matrix = new Matrix<DecimalMatrixItem>(3, 4);
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    matrix[i, j] = new DecimalMatrixItem((decimal)((i + j + 1) * (j + 1) * 0.001));
                }
            }
            Console.WriteLine("Matrix");
            Console.WriteLine(matrix);

            var otherMatrix = new Matrix<DecimalMatrixItem>(4, 3);
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    otherMatrix[i, j] = new DecimalMatrixItem((decimal)((i + j + 1) * (j + 1) * 0.001));
                }
            }
            Console.WriteLine("\nOther matrix");
            Console.WriteLine(otherMatrix);

            Console.WriteLine("\nTranspose matrix");
            Console.WriteLine(matrix.Transpose());
            Console.WriteLine("\nTranspose other matrix");
            Console.WriteLine(otherMatrix.Transpose());

            Console.WriteLine("\nMultiply matrixes");
            Console.WriteLine(matrix.Multiply(otherMatrix));
        }

        public static void ShowStack()
        {
            var stack = new Stack();
            CollectionChangeEventHandler showInfo = (sender, e) =>
            {
                Console.WriteLine($"Top = {stack.Top}, Bottom = {stack.Bottom}, BeginOfFrame = {stack.BeginOfFrame}");
                ShowAction($"{e.Action}: {e.Element}", stack);
            };
            stack.PushEvent += showInfo;
            stack.PopEvent += showInfo;
            stack.Push("top");
            stack.Push(true);
            stack.Push(new byte[] {12, 56, 90}, 3);
            var str = "QQQ";
            stack.Push(str);
            Console.WriteLine(stack.Pop());
            Console.WriteLine(stack.Pop(3));
            Console.WriteLine(stack.Pop());
        }

        public static void ShowStackTemplate()
        {
            var stack = new Tasks.Stack<string>();
            Console.WriteLine("Push 'first'");
            stack.Push("first");
            Console.WriteLine($"Top - {stack.Top}, bottom - {stack.Bottom}");
            Console.WriteLine("Push 'second'");
            stack.Push("second");
            Console.WriteLine($"Top - {stack.Top}, bottom - {stack.Bottom}");
            Console.WriteLine("Push 'third'");
            stack.Push("third");
            Console.WriteLine($"Top - {stack.Top}, bottom - {stack.Bottom}");
            Console.WriteLine("Pop");
            stack.Pop();
            Console.WriteLine($"Top - {stack.Top}, bottom - {stack.Bottom}");
            Console.WriteLine("Clear");
            try
            {
                stack.Clear();
                Console.WriteLine($"Top - {stack.Top}, bottom - {stack.Bottom}");
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            Console.WriteLine("Push 'aaaaaaaaaa'");
            stack.Push("aaaaaaaaaaaa");
            Console.WriteLine($"Top - {stack.Top}, bottom - {stack.Bottom}");
            Console.WriteLine("Push 'bbbbbbbbb'");
            stack.Push("bbbbbbbbbbbbb");
            Console.WriteLine($"Top - {stack.Top}, bottom - {stack.Bottom}");
            Console.WriteLine($"Top - {stack.Top}, bottom - {stack.Bottom}");
        }
    }
}
