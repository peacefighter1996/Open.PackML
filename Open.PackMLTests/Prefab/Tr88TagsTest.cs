using Autabee.Utility;
using Autabee.Utility.IEC61131TypeConversion;
using Moq;
using Open.PackML;
using Open.PackML.Prefab;
using Open.PackML.Tags;
using Open.PackML.Tags.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                if (string.Compare(tagArray[4], "X", StringComparison.Ordinal) != 0 && tr88)
                {
                    continue;
                }
                string tagtypestring = string.Join(", ", tagArray[0].Split('/'));
                tagConfig.TagType = (TagType)Enum.Parse(typeof(TagType), tagtypestring);
                tagConfig.Name = tagArray[1];
                tagConfig.EndUserTerm = tagArray[2];

                Assert.True(IECType.ContainsType(tagArray[3]), $"Dataset Incorrect, unkown type: {tagArray[3]}");

                var lastPart = tagArray[1].Split('.').Last();
                Type collectiontype = null;
                if (lastPart.Contains("[") && lastPart.Contains("]"))
                {
                    var number = lastPart.Split('[')[1].Split(']')[0];
                    collectiontype = string.Compare(number, "#", StringComparison.Ordinal) == 0 ? typeof(List<>) : typeof(Array);
                }

                tagConfig.DataType = IecTypeConvertor.GetCsharpType(tagArray[3], collectiontype, Assembly.GetAssembly(typeof(PmlTr88Controller)));
                tagConfigList.Add(tagConfig);
            }
            return tagConfigList.ToArray();
        }

        //Test if when a tagcontroller uses the Tr88 controller, has all the tags specified by PackML Implementation Guide
        [Fact()]
        public void TestTr88Tags()
        {
            var moqController = new Mock<IPmlController>();
            var table = TagTreeBuilder.GetTree(string.Empty, new PmlTr88Controller(moqController.Object, new PmlEventStore(), new PmlOemTransitionCheck())).BuildTable();
            //var tags = table.GetTags;
            logger.WriteLine(table.GetTagTablePrint());
            foreach (var tag in GetTagConfigList(true))
            {
                if (table.TryGetValue(tag.Name, out TagConfig data))
                {
                    if ((tag.Name.Equals(data.Name))
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

        [Fact()]
        public void TestEumTags()
        {
            var moqController = new Mock<IPmlController>();
            var table = TagTreeBuilder.GetTree(string.Empty, new PmlEumController(moqController.Object, new PmlEventStore(), new PmlOemTransitionCheck()), Iec: true).BuildTable();
            //var tags = table.GetTags;
            logger.WriteLine(table.GetTagTablePrint(Iec: true));
            var result = new ValidationResult();
            foreach (var tag in GetTagConfigList(false))
            {
                if (table.TryGetValue(tag.SearchString, out TagConfig data))
                {
                    if ((tag.Name.Equals(data.Name))
                        && (tag.EndUserTerm.Equals(data.EndUserTerm))
                        && (tag.TagType.Equals(data.TagType))
                        && (tag.DataType.Equals(data.DataType)))
                    {

                        continue;
                    }
                    else
                    {
                        result.AddResult(false, $"tag does not match: [{tag.Name}]==[{data.Name}], [{tag.EndUserTerm}]==[{data.EndUserTerm}], [{tag.TagType}]==[{data.TagType}], [{tag.DataType}]==[{data.DataType}]");
                    }
                }
                else
                {
                    result.AddResult(false, $"Tag [{tag.Name}] does not Exist");
                }
            }
            Assert.True(result.Success, result.FailString());
        }
    }
}
