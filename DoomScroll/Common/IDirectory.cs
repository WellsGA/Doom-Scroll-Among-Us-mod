using DoomScroll.UI;

namespace DoomScroll.Common
{
   public interface IDirectory
    {
        public string GetName();
        public string GetPath();
        public CustomButton GetButton();
        public void DisplayContent();
        public void HideContent();
        public string PrintDirectory();
    }
}
