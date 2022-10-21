using Autabee.Utility.IEC61131TypeConversion;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using Moq;
using Open.PackML;
using Open.PackML.Prefab;
using Open.PackML.Tags;
using Open.PackML.Tags.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Open.PackMLTests.Prefab
{
    
    public class Tr88TagsTest
    {
        ITestOutputHelper logger;
        public Tr88TagsTest(ITestOutputHelper output)
        {
            logger = output;
        }

        //Read tagDataset.csv and change them to tagconfig list
        public TagConfig[] GetTagConfigList(bool tr88)
        {
            var tagConfigList = new List<TagConfig>();
            var tagDataset = System.IO.File.ReadAllLines(@"Prefab/TagDataSet.csv");
            foreach (var tag in tagDataset)
            {
                var tagConfig = new TagConfig();
                var tagArray = tag.Split(',');
                if (tagArray[4] != "X" && tr88)
                {
                    continue;
                }
                tagConfig.TagType = Enum.Parse<TagType>(tagArray[0]);
                tagConfig.Name = tagArray[1];
                tagConfig.EndUserTerm = tagArray[2];

                Assert.True(IECType.ContainsType(tagArray[3]), $"Dataset Incorrect, unkown type: {tagArray[3]}");


                tagConfig.DataType = IecTypeConvertor.GetCsharpType(tagArray[3]);
                tagConfigList.Add(tagConfig);
            }
            return tagConfigList.ToArray();
        }



        //Test if when a tagcontroller uses the Tr88 controller, has all the tags specified by PackML Implementation Guide
        [Fact()]
        public void TestTr88Tags()
        {
            var moqController = new Mock<IPmlController>();
            var table = TagTreeBuilder.GetTree("", new PmlTr88Controller(moqController.Object, new PmlEventStore(), new PmlOemTransitionCheck())).BuildTable();
            //var tags = table.GetTags;
            logger.WriteLine(table.GetTagTablePrint());
            foreach (var tag in GetTagConfigList(true))
            {
                if (table.TryGetValue(tag.Name, out TagConfig data))
                {
                    if((tag.Name.Equals(data.Name))
                        && (tag.EndUserTerm.Equals(data.EndUserTerm))
                        && (tag.TagType.Equals(data.TagType))
                        && (tag.DataType.Equals(data.DataType)))
                    {
                        
                        Assert.True(true);
                    }
                    else
                    {
                        Assert.True(false, $"tag does not match: {tag.Name}, {tag.EndUserTerm}, {tag.TagType}, {tag.DataType}");
                    }
                }
                else
                {
                    Assert.True(false, $"tag does not match: {tag.Name}, {tag.EndUserTerm}, {tag.TagType}, {tag.DataType}");


                }
                
            }
        }
    }
}
