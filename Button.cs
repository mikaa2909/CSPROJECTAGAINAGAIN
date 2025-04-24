using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MAZEGAME
{
  public class Button : Component
  {

    private MouseState _currentMouse;
    private bool _isHovering;
    private MouseState _previousMouse;
    private Texture2D _texture;
    private Texture2D _hovertexture;
    public event EventHandler Click;
    public bool Clicked { get; private set; }
    public Vector2 Position { get; set; }
    public Rectangle Rectangle
    {
      get
      {
        return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
      }
    }
     public Rectangle HoverRectangle  
    {
      get
      {
        return new Rectangle((int)Position.X, (int)Position.Y, _hovertexture.Width, _hovertexture.Height);
      }
    }
    public Button(Texture2D texture, Texture2D hovertexture)
    {
      _texture = texture;
      _hovertexture = hovertexture;
    } 


    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      var colour = Color.White;

      if (_isHovering)
      {
        colour = Color.White;

        spriteBatch.Draw(_hovertexture, HoverRectangle, colour);
      }
      else
      {
        spriteBatch.Draw(_texture, Rectangle, colour);
      } 
  
    }

    public override void Update(GameTime gameTime) 
    {
      _previousMouse = _currentMouse; 
      _currentMouse = Mouse.GetState();

      var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

      _isHovering = false;

      if(mouseRectangle.Intersects(Rectangle))
      {
        _isHovering = true;

        if(_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed) 
        {
          Click?.Invoke(this, new EventArgs()); 
        }
      }
    }
    
  }
}