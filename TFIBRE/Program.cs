using System;
using System.Collections.Generic;

class Node
{
    public int index;
    public string name;
    public List<Node> connections;
    public Node parent;
    public int rank;

    public Node(int i, string n)
    {
        index = i;
        name = n;
        connections = new List<Node>();
        parent = this;
        rank = 0;
    }

    public void AddConnection(Node nodeAddress)
    {
        connections.Add(nodeAddress);
    }

    public Node Find()
    {
        if (this.parent != this)
            this.parent = this.parent.Find();
        return this.parent;
    }

    public void Union(Node node)
    {
        Node root1 = this.Find();
        Node root2 = node.Find();

        if (root1 == root2)
            return;

        if (root1.rank < root2.rank)
        {
            root1.parent = root2;
        }
        else if (root1.rank > root2.rank)
        {
            root2.parent = root1;
        }
        else
        {
            root2.parent = root1;
            root1.rank++;
        }
    }

    public bool IsConnectedTo(Node node)
    {
        return this.Find() == node.Find();
    }
}

class Program
{
    static Dictionary<string, Node> network = new Dictionary<string, Node>();

    static void AddNode(string name)
    {
        if (!network.ContainsKey(name))
        {
            Node newNode = new Node(network.Count, name);
            network.Add(name, newNode);
        }
    }

    static void AddConnection(string addressA, string addressB)
    {
        Node nodeA = network[addressA];
        Node nodeB = network[addressB];
        nodeA.AddConnection(nodeB);
        nodeB.AddConnection(nodeA);
        nodeA.Union(nodeB);
    }

    static bool CheckConnection(string addressA, string addressB)
    {
        if (!network.ContainsKey(addressA) || !network.ContainsKey(addressB))
            return false;

        Node nodeA = network[addressA];
        Node nodeB = network[addressB];

        return nodeA.IsConnectedTo(nodeB);
    }

    static void Main(string[] args)
    {
        string line;
        while ((line = Console.ReadLine()) != null)
        {
            string[] parts = line.Split();
            char operation = parts[0][0];
            string addressA = parts[1];
            string addressB = parts[2];

            switch (operation)
            {
                case 'B':
                    AddNode(addressA);
                    AddNode(addressB);
                    AddConnection(addressA, addressB);
                    break;

                case 'T':
                    if (CheckConnection(addressA, addressB))
                        Console.WriteLine("T");
                    else
                        Console.WriteLine("N");
                    break;
            }
        }
    }
}
