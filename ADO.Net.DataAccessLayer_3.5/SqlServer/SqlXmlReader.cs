using System;
using System.Data.SqlClient;
using System.Xml;

namespace DataAccessLayer.SqlServer
{
    /// <summary>
    /// Proxy class over XmlReader to allow control over the underlying SqlConnection
    /// </summary>
    /// <remarks>This class has been created to allow the control over the underlying SqlConnection so that when used in a
    /// using block the connection is closed when the reader is disposed removing the need for the client to have to handle
    /// the connection separately</remarks>
    public class SqlXmlReader : XmlReader
    {
        private readonly SqlConnection connection;
        private readonly XmlReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlXmlReader"/> class.
        /// </summary>
        /// <param name="cmd">The CMD.</param>
        public SqlXmlReader(SqlCommand cmd)
        {
            if (cmd == null)
                throw new ArgumentNullException("cmd");
            connection = cmd.Connection;
            reader = cmd.ExecuteXmlReader();
        }

        /// <summary>
        /// When overridden in a derived class, changes the <see cref="P:System.Xml.XmlReader.ReadState"/> to Closed.
        /// </summary>
        public override void Close()
        {
            reader.Close();
            connection.Close();
        }

        /// <summary>
        /// When overridden in a derived class, gets the number of attributes on the current node.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The number of attributes on the current node.
        /// </returns>
        public override int AttributeCount
        {
            get { return reader.AttributeCount; }
        }

        /// <summary>
        /// When overridden in a derived class, gets the base URI of the current node.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The base URI of the current node.
        /// </returns>
        public override string BaseURI
        {
            get { return reader.BaseURI; }
        }

        /// <summary>
        /// When overridden in a derived class, gets the depth of the current node in the XML document.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The depth of the current node in the XML document.
        /// </returns>
        public override int Depth
        {
            get { return reader.Depth; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the reader is positioned at the end of the stream.
        /// </summary>
        /// <value></value>
        /// <returns>true if the reader is positioned at the end of the stream; otherwise, false.
        /// </returns>
        public override bool EOF
        {
            get { return reader.EOF; }
        }

        /// <summary>
        /// When overridden in a derived class, gets the value of the attribute with the specified index.
        /// </summary>
        /// <param name="i">The index of the attribute. The index is zero-based. (The first attribute has index 0.)</param>
        /// <returns>
        /// The value of the specified attribute. This method does not move the reader.
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="i"/> is out of range. It must be non-negative and less than the size of the attribute collection.
        /// </exception>
        public override string GetAttribute(int i)
        {
            return reader.GetAttribute(i);
        }

        /// <summary>
        /// When overridden in a derived class, gets the value of the attribute with the specified <see cref="P:System.Xml.XmlReader.LocalName"/> and <see cref="P:System.Xml.XmlReader.NamespaceURI"/>.
        /// </summary>
        /// <param name="name">The local name of the attribute.</param>
        /// <param name="namespaceURI">The namespace URI of the attribute.</param>
        /// <returns>
        /// The value of the specified attribute. If the attribute is not found or the value is String.Empty, null is returned. This method does not move the reader.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="name"/> is null.
        /// </exception>
        public override string GetAttribute(string name, string namespaceURI)
        {
            return reader.GetAttribute(name, namespaceURI);
        }

        /// <summary>
        /// When overridden in a derived class, gets the value of the attribute with the specified <see cref="P:System.Xml.XmlReader.Name"/>.
        /// </summary>
        /// <param name="name">The qualified name of the attribute.</param>
        /// <returns>
        /// The value of the specified attribute. If the attribute is not found or the value is String.Empty, null is returned.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="name"/> is null.
        /// </exception>
        public override string GetAttribute(string name)
        {
            return reader.GetAttribute(name);
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current node can have a <see cref="P:System.Xml.XmlReader.Value"/>.
        /// </summary>
        /// <value></value>
        /// <returns>true if the node on which the reader is currently positioned can have a Value; otherwise, false. If false, the node has a value of String.Empty.
        /// </returns>
        public override bool HasValue
        {
            get { return reader.HasValue; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current node is an empty element (for example, &lt;MyElement/&gt;).
        /// </summary>
        /// <value></value>
        /// <returns>true if the current node is an element (<see cref="P:System.Xml.XmlReader.NodeType"/> equals XmlNodeType.Element) that ends with /&gt;; otherwise, false.
        /// </returns>
        public override bool IsEmptyElement
        {
            get { return reader.IsEmptyElement; }
        }

        /// <summary>
        /// When overridden in a derived class, gets the local name of the current node.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The name of the current node with the prefix removed. For example, LocalName is book for the element &lt;bk:book&gt;.
        /// For node types that do not have a name (like Text, Comment, and so on), this property returns String.Empty.
        /// </returns>
        public override string LocalName
        {
            get { return reader.LocalName; }
        }

        /// <summary>
        /// When overridden in a derived class, resolves a namespace prefix in the current element's scope.
        /// </summary>
        /// <param name="prefix">The prefix whose namespace URI you want to resolve. To match the default namespace, pass an empty string.</param>
        /// <returns>
        /// The namespace URI to which the prefix maps or null if no matching prefix is found.
        /// </returns>
        public override string LookupNamespace(string prefix)
        {
            return reader.LookupNamespace(prefix);
        }

        /// <summary>
        /// When overridden in a derived class, moves to the attribute with the specified <see cref="P:System.Xml.XmlReader.LocalName"/> and <see cref="P:System.Xml.XmlReader.NamespaceURI"/>.
        /// </summary>
        /// <param name="name">The local name of the attribute.</param>
        /// <param name="ns">The namespace URI of the attribute.</param>
        /// <returns>
        /// true if the attribute is found; otherwise, false. If false, the reader's position does not change.
        /// </returns>
        public override bool MoveToAttribute(string name, string ns)
        {
            return reader.MoveToAttribute(name, ns);
        }

        /// <summary>
        /// When overridden in a derived class, moves to the attribute with the specified <see cref="P:System.Xml.XmlReader.Name"/>.
        /// </summary>
        /// <param name="name">The qualified name of the attribute.</param>
        /// <returns>
        /// true if the attribute is found; otherwise, false. If false, the reader's position does not change.
        /// </returns>
        public override bool MoveToAttribute(string name)
        {
            return reader.MoveToAttribute(name);
        }

        /// <summary>
        /// When overridden in a derived class, moves to the element that contains the current attribute node.
        /// </summary>
        /// <returns>
        /// true if the reader is positioned on an attribute (the reader moves to the element that owns the attribute); false if the reader is not positioned on an attribute (the position of the reader does not change).
        /// </returns>
        public override bool MoveToElement()
        {
            return reader.MoveToElement();
        }

        /// <summary>
        /// When overridden in a derived class, moves to the first attribute.
        /// </summary>
        /// <returns>
        /// true if an attribute exists (the reader moves to the first attribute); otherwise, false (the position of the reader does not change).
        /// </returns>
        public override bool MoveToFirstAttribute()
        {
            return reader.MoveToFirstAttribute();
        }

        /// <summary>
        /// When overridden in a derived class, moves to the next attribute.
        /// </summary>
        /// <returns>
        /// true if there is a next attribute; false if there are no more attributes.
        /// </returns>
        public override bool MoveToNextAttribute()
        {
            return reader.MoveToNextAttribute();
        }

        /// <summary>
        /// When overridden in a derived class, gets the <see cref="T:System.Xml.XmlNameTable"/> associated with this implementation.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The XmlNameTable enabling you to get the atomized version of a string within the node.
        /// </returns>
        public override XmlNameTable NameTable
        {
            get { return reader.NameTable; }
        }

        /// <summary>
        /// When overridden in a derived class, gets the namespace URI (as defined in the W3C Namespace specification) of the node on which the reader is positioned.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The namespace URI of the current node; otherwise an empty string.
        /// </returns>
        public override string NamespaceURI
        {
            get { return reader.NamespaceURI; }
        }

        /// <summary>
        /// When overridden in a derived class, gets the type of the current node.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// One of the <see cref="T:System.Xml.XmlNodeType"/> values representing the type of the current node.
        /// </returns>
        public override XmlNodeType NodeType
        {
            get { return reader.NodeType; }
        }

        /// <summary>
        /// When overridden in a derived class, gets the namespace prefix associated with the current node.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The namespace prefix associated with the current node.
        /// </returns>
        public override string Prefix
        {
            get { return reader.Prefix; }
        }

        /// <summary>
        /// When overridden in a derived class, reads the next node from the stream.
        /// </summary>
        /// <returns>
        /// true if the next node was read successfully; false if there are no more nodes to read.
        /// </returns>
        /// <exception cref="T:System.Xml.XmlException">
        /// An error occurred while parsing the XML.
        /// </exception>
        public override bool Read()
        {
            return reader.Read();
        }

        /// <summary>
        /// When overridden in a derived class, parses the attribute value into one or more Text, EntityReference, or EndEntity nodes.
        /// </summary>
        /// <returns>
        /// true if there are nodes to return.
        /// false if the reader is not positioned on an attribute node when the initial call is made or if all the attribute values have been read.
        /// An empty attribute, such as, misc="", returns true with a single node with a value of String.Empty.
        /// </returns>
        public override bool ReadAttributeValue()
        {
            return reader.ReadAttributeValue();
        }

        /// <summary>
        /// When overridden in a derived class, gets the state of the reader.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// One of the <see cref="T:System.Xml.ReadState"/> values.
        /// </returns>
        public override ReadState ReadState
        {
            get { return reader.ReadState; }
        }

        /// <summary>
        /// When overridden in a derived class, resolves the entity reference for EntityReference nodes.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">
        /// The reader is not positioned on an EntityReference node; this implementation of the reader cannot resolve entities (<see cref="P:System.Xml.XmlReader.CanResolveEntity"/> returns false).
        /// </exception>
        public override void ResolveEntity()
        {
            reader.ResolveEntity();
        }

        /// <summary>
        /// When overridden in a derived class, gets the text value of the current node.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The value returned depends on the <see cref="P:System.Xml.XmlReader.NodeType"/> of the node. The following table lists node types that have a value to return. All other node types return String.Empty.
        /// Node type  /// Value
        /// Attribute
        /// The value of the attribute.
        /// CDATA
        /// The content of the CDATA section.
        /// Comment
        /// The content of the comment.
        /// DocumentType
        /// The internal subset.
        /// ProcessingInstruction
        /// The entire content, excluding the target.
        /// SignificantWhitespace
        /// The white space between markup in a mixed content model.
        /// Text
        /// The content of the text node.
        /// Whitespace
        /// The white space between markup.
        /// XmlDeclaration
        /// The content of the declaration.
        /// </returns>
        public override string Value
        {
            get { return reader.Value; }
        }
    }
}
