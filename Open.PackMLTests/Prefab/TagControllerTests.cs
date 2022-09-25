using Xunit;
using Open.PackML.Tags.Prefab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Open.PackML.Prefab;
using Open.PackML.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

namespace Open.PackMLTests.Prefab
{
    public class TagControllerTests
    {

        
        [Fact()]
        public void GetTagDataTest()
        {
            var moqController = new Mock<IPmlController<Enum>>();
            var eventStore = new EventStore();
            var eumController = new PackMLEumController<Enum>(moqController.Object, eventStore);
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

        [Fact()]
        public void SetArrayTagDataTest()
        {
            TagController controller = GetTestController();
            var result = controller.SetTagData("Machine1.IntegerArray2[0]", 45);
            Assert.True(result.Success, result.FailString());
            var value  = controller.GetTagData<int>("Machine1.IntegerArray2[0]");
            Assert.True(value.Success, value.FailString());
            Assert.Equal(45, value.Object);
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

        private static TagController GetDefaultcontroller()
        {
            var moqController = new Mock<IPmlController<Enum>>();
            var eventStore = new EventStore();
            var eumController = new PackMLEumController<Enum>(moqController.Object, eventStore);
            var controller = new TagController(new Dictionary<string, object>() { { "", eumController } });
            return controller;
        }

        private static TagController GetTestController()
        {
            var controller = new TagController(new Dictionary<string, object>() { 
                { "Machine1", new TestObject1() }, 
                { "Machine2", new TestObject2() } 
            });
            return controller;
        }
    }
}