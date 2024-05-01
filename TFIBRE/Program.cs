using System;
using System.Collections.Generic;

class Node
{
    public bool visited;
    public int index;
    public string name;
    public List<Node> connections;

    public Node(int i, string n)
    {
        index = i;
        name = n;
        connections = new List<Node>();
    }

    public void PutNewConnection(Node nodeAddress)
    {
        connections.Add(nodeAddress);
    }

    public void Flood(string seekNodeName)
    {
        if (visited || Program.nodeIsFound)
            return;

        visited = true;

        foreach (Node connection in connections)
        {
            if (connection.name == seekNodeName)
            {
                connection.visited = true;
                Program.nodeIsFound = true;
                break;
            }
            else
                connection.Flood(seekNodeName);
        }
    }

    public void ShowConnections()
    {
        foreach (Node connection in connections)
        {
            Console.WriteLine($"{name} has connection with: {connection.name}");
        }
    }
}

class Program
{
    public static bool nodeIsFound;
    static string seekNodeName;
    static List<string> addTab = new List<string>();
    static List<Node> nodesTab = new List<Node>();

    static void PutNewAddress(string address)
    {
        if (!addTab.Contains(address))
        {
            int indx = nodesTab.Count;
            addTab.Add(address);
            nodesTab.Add(new Node(indx, address));
        }
    }

    static Tuple<int, int> ReturnIndexOfAddress(string addA, string addB)
    {
        int indexA = -1, indexB = -1;

        for (int i = 0; i < addTab.Count; i++)
        {
            if (indexA != -1 && indexB != -1)
                break;
            else
            {
                if (addTab[i] == addA)
                    indexA = i;
                if (addTab[i] == addB)
                    indexB = i;
            }
        }

        return Tuple.Create(indexA, indexB);
    }

    static void PutNewConnection(string addA, string addB)
    {
        Tuple<int, int> indexes = ReturnIndexOfAddress(addA, addB);
        int indexA = indexes.Item1;
        int indexB = indexes.Item2;

        Node AAddress = nodesTab[indexA];
        Node BAddress = nodesTab[indexB];

        BAddress.PutNewConnection(AAddress);
        AAddress.PutNewConnection(BAddress);
    }

    static bool CheckConnection(string addA, string addB)
    {
        nodeIsFound = false;
        seekNodeName = addB;

        Tuple<int, int> indexes = ReturnIndexOfAddress(addA, addB);
        int indexA = indexes.Item1;
        int indexB = indexes.Item2;

        if (indexA == -1 || indexB == -1)
            return false;

        foreach (Node node in nodesTab)
        {
            node.visited = false;
        }

        nodesTab[indexA].Flood(seekNodeName);

        return nodesTab[indexB].visited;
    }

    static void Main(string[] args)
    {
        char opCode;
        while (char.TryParse(Console.ReadLine(), out opCode))
        {
            string addA = Console.ReadLine();
            string addB = Console.ReadLine();

            switch (opCode)
            {
                case 'B':
                    PutNewAddress(addA);
                    PutNewAddress(addB);
                    PutNewConnection(addA, addB);
                    break;

                case 'T':
                    if (CheckConnection(addA, addB))
                        Console.WriteLine("T");
                    else
                        Console.WriteLine("N");
                    break;
            }
        }
    }
}