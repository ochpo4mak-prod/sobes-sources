using System;

namespace Assets.Scripts.Table
{
    public class Node : INode, IContent
    {
        public Node(ENodeContent content)
        {
            Content = content;
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; }
        public ENodeContent Content { get; }
    }
}
