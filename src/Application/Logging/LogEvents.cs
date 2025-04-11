public enum LogEvent
{
    GetAll = 1000,
    GetAllNotFound = 1001,
    GetById = 1002,
    GetByIdNotFound = 1003,
    GetByField = 1004,
    GetByFieldNotFound = 1005,
    AddEntity = 1006,
    AddNullEntity = 1007,
    UpdateEntity = 1008,
    UpdateNullEntity = 1009,
    DeleteEntity = 1010,
    DeleteNullEntity = 1011,
}