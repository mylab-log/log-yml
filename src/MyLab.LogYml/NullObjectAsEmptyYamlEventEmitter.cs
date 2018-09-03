using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.EventEmitters;

namespace MyLab.LogYml
{
    class NullObjectAsEmptyYamlEventEmitter : ChainedEventEmitter
    {
        public NullObjectAsEmptyYamlEventEmitter(IEventEmitter nextEmitter)
            : base(nextEmitter)
        {
        }

        public override void Emit(ScalarEventInfo eventInfo, IEmitter emitter)
        {
            if (eventInfo.Source.Value == null)
            {
                emitter.Emit(new Scalar(string.Empty));
            }
            else
            {
                base.Emit(eventInfo, emitter);
            }
        }
    }
}