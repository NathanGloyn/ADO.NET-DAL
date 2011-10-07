using System;
using System.Xml;
using DataAccessLayer.SqlServer;
using NUnit.Framework;

namespace ADO.Net.DataAccessLayer.SqlServer.Tests
{
    [TestFixture]
    public class When_using_XmlReader:CommonTestSetup
    {
        [Test]
        public void Should_throw_ArgumentNullException_if_no_SqlCommand_provided()
        {
            Assert.Throws<ArgumentNullException>(() => new SqlXmlReader(null));
        }

        [Test]
        public void Should_have_correct_baseURI()
        {
            using (XmlReader reader = DataAccess.ExecuteXmlReader("SelectAllFromTestTableAsXml"))
            {
                Assert.IsTrue(reader.BaseURI == string.Empty);
            }
        }

        [Test]
        public void Should_return_EOF_when_reach_last_element()
        {
            using (XmlReader reader = DataAccess.ExecuteXmlReader("SelectAllFromTestTableAsXml"))
            {
                while (!reader.EOF)
                {
                    reader.Read();
                }

                Assert.IsTrue(reader.EOF);
            }
        }

        [Test]
        public void Should_read_element()
        {
            using (XmlReader reader = DataAccess.ExecuteXmlReader("SelectAllFromTestTableAsXml"))
            {
                Assert.IsTrue(reader.Read());
            }              
        }

        [Test]
        public void Should_return_true_when_reader_HasAttributes_Called()
        {
            using (XmlReader reader = DataAccess.ExecuteXmlReader("SelectAllFromTestTableAsXmlUsingAttributes"))
            {
                reader.Read();
                reader.ReadStartElement();
                Assert.IsTrue(reader.HasAttributes);
            }            
        }

        [Test]
        public void Should_return_zero_for_attribute_count_for_element_with_no_attributes()
        {
            using (XmlReader reader = DataAccess.ExecuteXmlReader("SelectAllFromTestTableAsXmlUsingAttributes"))
            {
                reader.Read();
                Assert.IsTrue(reader.AttributeCount == 0);
            }                
        }

        [Test]
        public void Should_return_correct_attribute_count_for_element_with_attributes()
        {
            using (XmlReader reader = DataAccess.ExecuteXmlReader("SelectAllFromTestTableAsXmlUsingAttributes"))
            {
                reader.Read();
                reader.ReadStartElement();
                Assert.IsTrue(reader.AttributeCount == 2);
            }                
        }

        [Test]
        public void Should_be_able_to_move_to_attribute_by_name()
        {
            using (XmlReader reader = DataAccess.ExecuteXmlReader("SelectAllFromTestTableAsXmlUsingAttributes"))
            {
                reader.Read();
                reader.ReadStartElement();
                Assert.IsTrue(reader.MoveToAttribute("Key"));
            }
        }

        [Test]
        public void Should_be_able_to_move_to_attribute_by_name_and_namespace()
        {
            using (XmlReader reader = DataAccess.ExecuteXmlReader("SelectTestTableAsXmlWithAttributesUsingNamespaces"))
            {
                reader.Read();
                reader.ReadStartElement();
                Assert.IsTrue(reader.MoveToAttribute("Key","uri"));
            }
        }

        [Test]
        public void Should_be_able_to_get_attribute_value_by_index()
        {
            using (XmlReader reader = DataAccess.ExecuteXmlReader("SelectAllFromTestTableAsXmlUsingAttributes"))
            {
                reader.Read();
                reader.ReadStartElement();
                Assert.IsTrue(reader.GetAttribute(0) == "key1");
            }            
        }

        [Test]
        public void Should_be_able_to_get_attribute_value_by_name()
        {
            using (XmlReader reader = DataAccess.ExecuteXmlReader("SelectAllFromTestTableAsXmlUsingAttributes"))
            {
                reader.Read();
                reader.ReadStartElement();
                Assert.IsTrue(reader.GetAttribute("Key") == "key1");
            }
        }

        [Test]
        public void Should_be_able_to_get_attribute_value_by_name_and_namespace()
        {
            using (XmlReader reader = DataAccess.ExecuteXmlReader("SelectTestTableAsXmlWithAttributesUsingNamespaces"))
            {
                reader.Read();
                reader.ReadStartElement();
                Assert.IsTrue(reader.GetAttribute("Key","uri") == "key1");
            }
        }

        [Test]
        public void Should_be_able_to_get_namespace()
        {
            using (XmlReader reader = DataAccess.ExecuteXmlReader("SelectTestTableAsXmlWithAttributesUsingNamespaces"))
            {
                reader.Read();
                reader.ReadStartElement();
                Assert.IsTrue(reader.LookupNamespace("ns1") == "uri");
            }
        }

        [Test]
        public void Should_be_able_to_move_to_next_attribute_in_element()
        {
            using (XmlReader reader = DataAccess.ExecuteXmlReader("SelectAllFromTestTableAsXmlUsingAttributes"))
            {
                reader.Read();
                reader.ReadStartElement();
                reader.MoveToAttribute("Key");
                Assert.IsTrue(reader.MoveToNextAttribute());
            }
        }

        [Test]
        public void Should_return_true_for_element_that_has_a_value()
        {
            using (XmlReader reader = DataAccess.ExecuteXmlReader("SelectAllFromTestTableAsXml"))
            {
                reader.ReadToDescendant("Key");
                reader.Read();
                Assert.IsTrue(reader.HasValue);
            }            
        }

        [Test]
        public void Should_be_able_to_read_attribute_value()
        {
            using (XmlReader reader = DataAccess.ExecuteXmlReader("SelectAllFromTestTableAsXmlUsingAttributes"))
            {
                reader.Read();
                reader.ReadStartElement();
                reader.MoveToAttribute("Key");
                Assert.IsTrue(reader.ReadAttributeValue());
            }
        }

        [Test]
        public void Should_move_to_element()
        {
            using (XmlReader reader = DataAccess.ExecuteXmlReader("SelectAllFromTestTableAsXmlUsingAttributes"))
            {
                reader.Read();
                reader.ReadStartElement();
                reader.ReadToDescendant("Key");
                reader.MoveToAttribute("Key");
                Assert.IsTrue(reader.MoveToElement());

            }
        }
    }
}