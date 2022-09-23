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

namespace Open.PackML.Tags.Prefab.Tests
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

            var value = controller.GetTagData("ProdDefectiveCount");
            
            Assert.Equal(45, value.Object);

            value = controller.GetTagData("ProdProcessedCount");

            Assert.Equal(163, value.Object);
        }

        [Fact()]
        public void SetTagDataTest()
        {
            var moqController = new Mock<IPmlController<Enum>>();
            var eventStore = new EventStore();
            var eumController = new PackMLEumController<Enum>(moqController.Object, eventStore);
            


            var controller = new TagController(new Dictionary<string, object>()
            {{"", eumController }});
            var result = controller.SetTagData("StopReason.ID", 45);
            Assert.True(result.Success, result.FailString());
            var value = controller.GetTagData("StopReason.ID");

            Assert.Equal(45, value.Object);
        }
    }
}