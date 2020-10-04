public class GameEventSystem {
    public delegate void EventRaised(GAME_EVENT type, System.Object data);
    public static event EventRaised GameEventHandler;
    public static void RaiseGameEvent(GAME_EVENT type, System.Object data = null) {
        GameEventHandler?.Invoke(type, data);
    }
}

public enum GAME_EVENT {
    LEVEL_START,
    PLATFORM_DESTROYED,
    OBSTACLE_DESTROYED,
    ENEMY_KILLED,
    REST_LEVEL,
    LEVEL_END
}

public enum LEVEL_END_REASON {
    TIMER_EXPIRED,
    OBJECTIVE_COLLECTED,
    PLAYER_DEAD
}