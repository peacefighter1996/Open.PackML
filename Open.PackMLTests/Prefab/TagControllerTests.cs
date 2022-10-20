using Xunit;
using Open.PackML.Tags.Prefab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Open.PackML.Prefab;
using Open.PackML;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using Open.PackML.Tags;
using Open.PackMLTests.TestObjects;

namespace Open.PackMLTests.Prefab
{
    public class TagControllerTests
    {

        [Fact()]
        public void GetTagDataTest()
        {
            var moqController = new Mock<IPmlController>();
            var eventStore = new PmlEventStore();
            var eumController = new PmlEumController(moqController.Object, eventStore);
            eumController.GetType().GetProperty("ProdDefectiveCount").SetValue(eumController, 45);
            eumController.GetType().GetProperty("ProdProcessedCount").SetValue(eumController, 163);


            var controller = new TagController(new Dictionary<string, object>()
            {{"", eumController }});

            var value = controller.GetTagData<int>("ProdDefectiveCount");

            Assert.Equal(45, value.Object);

            value = controller.GetTagData<int>("ProdProcessedCount");

            Assert.Equal(163, value.Object);
        }

        [Fact()]
        public void SetTagDataTest()
        {
            TagController controller = GetDefaultcontroller();
            var result = controller.SetTagData("StopReason.ID", 45);
            Assert.True(result.Success, result.FailString());
            var value = controller.GetTagData<int>("StopReason.ID");
            Assert.Equal(45, value.Object);
        }


        [Fact()]
        public void FailSetTagDataTest()
        {
            TagController controller = GetDefaultcontroller();
            var result = controller.SetTagData("StopReason", new PmlStopReason(45, 100));
            Assert.False(result.Success, result.FailString());
        }

        [Theory]
        [InlineData("Machine1.IntegerArray2[0]", 45)]
        [InlineData("Machine1.IntegerArray1[0]", 45)]
        [InlineData("Machine1.IntegerArray2[1]", 125)]
        public void SetArrayValueTagDataTest<T>(string tagName, T tagValue)
        {
            TagController controller = GetTestController();
            var result = controller.SetTagData(tagName, tagValue);
            Assert.True(result.Success, result.FailString());
            var value = controller.GetTagData<T>(tagName);
            Assert.True(value.Success, value.FailString());
            Assert.Equal(tagValue, value.Object);
        }



        [Fact()]
        public void SetArrayTagDataTest1FailWrite()
        {
            TagController controller = GetTestController();
            var result = controller.SetTagData("Machine1.IntegerArray1", new int[] { 112, 25564 });
            Assert.False(result.Success, result.FailString());
        }
        [Fact()]
        public void SetArrayTagDataTest2()
        {
            TagController controller = GetTestController();
            var result = controller.SetTagData("Machine1.IntegerArray2", new int[] { 112, 25564 });
            Assert.True(result.Success, result.FailString());
            var value = controller.GetTagData<int[]>("Machine1.IntegerArray2");
            Assert.True(value.Success, value.FailString());
            Assert.Equal(new int[] { 112, 25564 }, value.Object);
        }


        [Fact()]
        // Call a method with a parameter
        public void CallFunction()
        {
            TagController controller = GetTestController();
            var result = controller.ExecutePackTagCommand("Machine1.SetValue(DINT value1)", 45);
            Assert.True(result.Success, result.FailString());
            var value = controller.GetTagData<int>("Machine1.Integer1");
            Assert.True(value.Success, value.FailString());
            Assert.Equal(45, value.Object);
        }

        [Fact()]
        // Call a method with a parameter
        public void CallArrayVoidFunction()
        {
            TagController controller = GetTestController();
            for (int i = 0; i < 4; i++)
            {
                var result = controller.ExecutePackTagCommand($"Machine1.ExecuteObjects[{i}].SetValue(DINT value1)", 45);
                Assert.True(result.Success, result.FailString());
                var value = controller.GetTagData<int>($"Machine1.ExecuteObjects[{i}].Integer1");
                Assert.True(value.Success, value.FailString());
                Assert.Equal(45, value.Object);
            }

        }

        [Fact()]
        // Call a method with a parameter
        public void CallArrayIntFunction()
        {
            TagController controller = GetTestController();
            for (int i = 0; i < 4; i++)
            {
                var result = controller.ExecutePackTagCommand($"Machine1.ExecuteObjects[{i}].PlusOne(DINT value1)", 45);
                Assert.True(result.Success, result.FailString());
                Assert.Equal(result.Object, 46);
            }
        }

        [Fact()]
        // Call a method with a parameter
        public void CallListVoidFunction()
        {
            TagController controller = GetTestController();
            for (int i = 0; i < 3; i++)
            {
                var result = controller.ExecutePackTagCommand($"Machine2.ExecuteObjects[{i}].SetValue(DINT value1)", 45);
                Assert.True(result.Success, result.FailString());
                var value = controller.GetTagData<int>($"Machine2.ExecuteObjects[{i}].Integer1");
                Assert.True(value.Success, value.FailString());
                Assert.Equal(45, value.Object);
            }
        }

        [Fact()]
        // Call a method with a parameter
        public void CallListIntFunction()
        {
            TagController controller = GetTestController();
            for (int i = 0; i < 3; i++)
            {
                var result = controller.ExecutePackTagCommand($"Machine2.ExecuteObjects[{i}].PlusOne(DINT value1)", 45);
                Assert.True(result.Success, result.FailString());
                Assert.Equal(result.Object, 46);
            }
        }

        [Fact()]
        public void GetSetBatch()
        {
            var calls = new List<(string, object)>() {
                new ("Machine1.IntegerArray2[0]", 45),
                new ("Machine1.IntegerArray1[0]", 45),
                new ("Machine1.IntegerArray2[1]", 125)
            };

            var calls2 = new List<string>() {
                new ("Machine1.IntegerArray2[0]"),
                new ("Machine1.IntegerArray1[0]"),
                new ("Machine1.IntegerArray2[1]")
            };

            TagController controller = GetTestController();

            var result = controller.BatchSetTagData(calls);
            foreach (var item in result)
            {
                Assert.True(item.Success);
            }

            var result2 = controller.BatchGetTagData(calls2);

            for (int i = 0; i < calls.Count(); i++)
            {
                Assert.Equal(calls[i].Item2, result2[i].Object);
            }
        }

        [Fact()]
        // Call a method with a parameter
        public void CallBatchTagTypeFunction()
        {
            TagController controller = GetTestController();

            controller.SetTagData("Machine1.IntegerArray1[1]", 451);

            var result = controller.BatchCall(new List<TagCall>()
                {
                    new TagCall(){
                        TagName = $"Machine2.ExecuteObjects[0].PlusOne(DINT value1)",
                        Data = 45,
                        TagCallType = TagCallType.Execute
                    },
                    new TagCall(){
                        TagName = $"Machine2.ExecuteObjects[1].SetValue(DINT value1)",
                        Data = 45,
                        TagCallType = TagCallType.Execute
                    },
                    new TagCall(TagCallType.Set, "Machine1.IntegerArray1[0]", 45),
                    new TagCall(TagCallType.Get, "Machine1.IntegerArray1[1]")
                });

            Assert.True(result[0].Success, result[0].FailString());
            Assert.Equal(result[0].Object, 46);

            Assert.True(result[1].Success, result[1].FailString());
            Assert.Null(result[1].Object);

            Assert.True(result[2].Success, result[2].FailString());
            Assert.True(result[3].Success, result[3].FailString());

            Assert.Equal(451, result[3].Object);
        }


        private static TagController GetDefaultcontroller()
        {
            var moqController = new Mock<IPmlController>();
            var eventStore = new PmlEventStore();
            var eumController = new PmlEumController(moqController.Object, eventStore);
            var controller = new TagController(new Dictionary<string, object>() { { "", eumController } });
            return controller;
        }

        private static TagController GetTestController()
        {
            var controller = new TagController(new Dictionary<string, object>() {
                { "Machine1", new TestObject1() },
                { "Machine2", new TestObject2() }
            }, Iec: true);
            return controller;
        }
    }
}