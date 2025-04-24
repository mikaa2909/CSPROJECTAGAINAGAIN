using Microsoft.Xna.Framework;
namespace MAZEGAME
{
   public static class Numbers
    {
        public static Rectangle getRectangle(char number) {
            switch (number) {
                case '0': return new Rectangle(3, 48, 21, 21); 
                case '1': return new Rectangle(27, 48, 21, 21);
                case '2': return new Rectangle(51, 48, 21, 21); 
                case '3': return new Rectangle(75, 48, 21, 21); 
                case '4': return new Rectangle(99, 48, 21, 21); 
                case '5': return new Rectangle(123, 48, 21, 21); 
                case '6': return new Rectangle(147, 48, 21, 21); 
                case '7': return new Rectangle(171, 48, 21, 21); 
                case '8': return new Rectangle(195, 48, 21, 21); 
                case '9': return new Rectangle(219, 48, 21, 21);  
                default: return new Rectangle(3, 48, 21, 21); 
            }
        }
    }
}