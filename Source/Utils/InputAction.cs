using Godot;

namespace Utils
{
    public class InputAction
    {
        public static string GetPrimaryInputKeyText(string action)
        {
            var events = InputMap.ActionGetEvents(action);

            if (events.Count > 0)
            {
                var inputEvent = events[0];
                if (inputEvent is InputEventKey inputEventKey)
                {
                    return OS.GetKeycodeString(inputEventKey.PhysicalKeycode);
                }
            }

            return null;
        }
    }
}