using DIKUArcade.EventBus;

namespace exam_2018 {
    public static class SpaceTaxiBus {
        private static GameEventBus<object> eventBus;

        public static GameEventBus<object> GetBus() {
            return SpaceTaxiBus.eventBus ?? (SpaceTaxiBus.eventBus = new GameEventBus<object>());
        }
    }
}