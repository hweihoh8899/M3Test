Advantage:
- Scene hierarchy is clean.
- Has polymorphic attribute in item implementation (item, normal item, special item).
- Use scriptable object for game config.
- All panel implement IMenu.
Disadvantage and suggestion:
- Board script manage many things, can split board data, board operations, board finder, etc.
- Board Controller can separate the input controller,  
- Still Monobehavior action in Item script (I suppose its original purpose is a data class?), can remove instantiate, destroy, dotween
- 2 Ultility folder (Utilities and Utility)
- No namespace, simple classs name like cell, item, board can be duplicated when collaborating or use 3rd party packages
