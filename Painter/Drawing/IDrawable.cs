// using Painter.Drawables;
// using Painter.Drawables.DrawableImpl;
// using Painter.Events;
//
// namespace Painter.Drawing;
//
// public interface IDrawable
// {
//     /// <summary>
//     /// 
//     /// </summary>
//     public bool NeedsRedraw { get; set; }
//     
//     /// <summary>
//     /// The X position of this drawable relative to its parent
//     /// </summary>
//     public int X { get; set; }
//
//     /// <summary>
//     /// The Y position of this drawable relative to its parent
//     /// </summary>
//     public int Y { get; set; }
//
//     /// <summary>
//     /// The width of this drawable
//     /// </summary>
//     public int Width { get; set; }
//
//     /// <summary>
//     /// The maximum width of this drawable, null if none
//     /// </summary>
//     public int? MaxWidth { get; set; }
//
//     /// <summary>
//     /// The minimum width of this drawable
//     /// </summary>
//     public int MinWidth { get; set; }
//
//     /// <summary>
//     /// The height of this drawable
//     /// </summary>
//     public int Height { get; set; }
//
//     /// <summary>
//     /// The maximum height of this drawable, null if none
//     /// </summary>
//     public int? MaxHeight { get; set; }
//
//     /// <summary>
//     /// The minimum height of this drawable
//     /// </summary>
//     public int MinHeight { get; set; }
//     
//     /// <summary>
//     /// 
//     /// </summary>
//     public bool Visible { get; set; }
//
//     /// <summary>
//     /// Indicates whether this drawable can be focused
//     /// </summary>
//     public bool Focusable { set; get; }
//
//     /// <summary>
//     /// Indicates whether this drawable is focused
//     /// </summary>
//     public bool Focused { set; get; }
//
//     /// <summary>
//     /// The order in which the focus should cycle, lower values come first
//     /// </summary>
//     public uint FocusOrder { get; set; }
//
//     /// <summary>
//     /// 
//     /// </summary>
//     /// <param name="cycleDirection"></param>
//     /// <returns></returns>
//     public bool CycleFocus(CycleDirection cycleDirection);
//
//     /// <summary>
//     /// Draws this drawable to the provided <see cref="Canvas"/>
//     /// </summary>
//     /// <param name="canvas">The canvas to be drawn to</param>
//     public void Draw(Canvas canvas);
//
//     /// <summary>
//     /// 
//     /// </summary>
//     /// <param name="args"></param>
//     public void Interact(KeyPressedEventArgs args);
// }