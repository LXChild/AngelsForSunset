using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyApp
{
    public sealed class RemindDataTemplateSelector : DataTemplateSelector
    {

        /// <summary>
        /// 显示收到的消息的模板
        /// </summary>
        public DataTemplate MessageFromTemplate { get; set; }
        /// <summary>
        /// 显示已发送消息的模板
        /// </summary>
        public DataTemplate MessageToTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            RemindEntity msgent = item as RemindEntity;
            if (msgent != null)
            {
                // 判断消息类型，返回对应的模板
                if (msgent.MessageType == RemindEntity.MsgType.From)
                {
                    return MessageFromTemplate;
                }
                else
                {
                    return MessageToTemplate;
                }
            }
            return null;
        }
    }
}
