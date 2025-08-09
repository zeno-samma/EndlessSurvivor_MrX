## 📋 Tháp AsmDef
Assets/MrX/EndlessSurvivor/Scripts/
│
├── 00_Core/            (Tầng 1 - Nền Móng. AsmDef: Core)
│   ├── EventBus.cs
│   ├── GameState.cs
│   ├── IDamageable.cs
│   └── ...
│
├── 01_Features/        (Tầng 2 - Các Tính Năng)
│   ├── Player/         (AsmDef: Player)
│   ├── Enemies/        (AsmDef: Enemies)
│   ├── UI/             (AsmDef: UI)
│   └── Weapons/        (AsmDef: Weapons)
│
└── 02_Gameplay/        (Tầng 3 - Điều Phối. AsmDef: Gameplay)
    ├── GameManager.cs
    ├── EnemyManager.cs
    └── ...