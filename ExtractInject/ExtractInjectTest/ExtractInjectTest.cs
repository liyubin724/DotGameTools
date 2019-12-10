using ExtractInject;
using NUnit.Framework;

namespace ExtractInjectTest
{
    [TestFixture]
    public class ExtractInjectTest
    {
        private ExtractInjectContext context = null;

        [SetUp]
        public void Setup()
        {
            context = new ExtractInjectContext();
        }

        [TearDown]
        public void TearDown()
        {
            context.Clear();
            context = null;
        }

        [Test]
        public void TestContext()
        {
            EIObject obj = new EIObject();
            obj.AddToContext(context);

            EIObject obj2 = context.GetObject<EIObject>();
            Assert.IsNotNull(obj2, "TestContext->obj2 is null");
            Assert.AreSame(obj, obj2,"TestContext->Not Same");
        }

        [Test]
        public void TestInject()
        {
            EIObject obj = new EIObject();
            obj.AddToContext(context);

            InjectObject injectObject = new InjectObject();
            injectObject.Inject(context);

            Assert.IsNotNull(injectObject.eiObj, "Inject Object is Null");
            Assert.AreSame(injectObject.eiObj, obj, "Inject Object not Same");
        }

        [Test]
        public void TestExtract()
        {
            ExtractObject extractObject = new ExtractObject();
            extractObject.Extract(context);

            EIObject eIObject = context.GetObject<EIObject>();

            Assert.IsNotNull(eIObject, "Object is Null");
            Assert.AreEqual(11, eIObject.intValue, "Value not Equal");
        }
    }
}