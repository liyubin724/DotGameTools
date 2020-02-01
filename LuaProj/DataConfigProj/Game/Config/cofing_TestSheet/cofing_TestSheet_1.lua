require ("Dot/Config/ConfigSubSheet")
require ("Dot/Config/ConfigSheetLine")
local t_1 =function()
    return 1.0
end
local t_2 = {
    1,
    2,
    3,
}
local t_3 = {
    1,
    2,
    3,
    4,
}
local t_4 = {
    1,
    2,
    3,
    5,
}
local t_5 = {
    [1] = 1,
    [2] = 2,
}
local cofing_TestSheet_1 = {}
cofing_TestSheet_1.__index = cofing_TestSheet_1
cofing_TestSheet_1.ids = {1,4,5,8,9,11,12,14,20,31,32,54,111,}
cofing_TestSheet_1[1] = {
    ID = 1,
    LongValue = 1,
    FloatValue = 1,
    StringValue_Index = 1,
    TextValue = 1,
    ResValue_Index = 8,
    LuaValue = t_1,
    ArrayValue = t_2,
    DicValue = t_5,
}
cofing_TestSheet_1[1].__index = cofing_TestSheet_1[1]

cofing_TestSheet_1[4] = {
    ID = 4,
    StringValue_Index = 2,
    LuaValue = t_1,
    ArrayValue = t_3,
    DicValue = t_5,
}
cofing_TestSheet_1[4].__index = cofing_TestSheet_1[4]

cofing_TestSheet_1[5] = {
    ID = 5,
    LongValue = 65,
    BoolValue = false,
    StringValue_Index = 2,
    ResValue_Index = 1,
    LuaValue = t_1,
    ArrayValue = t_4,
    DicValue = t_5,
}
cofing_TestSheet_1[5].__index = cofing_TestSheet_1[5]

cofing_TestSheet_1[8] = {
    ID = 8,
    FloatValue = 1.4,
    BoolValue = false,
    LuaValue = t_1,
    ArrayValue = t_2,
    DicValue = t_5,
}
cofing_TestSheet_1[8].__index = cofing_TestSheet_1[8]

cofing_TestSheet_1[9] = {
    ID = 9,
    LongValue = 234,
    FloatValue = 11.1,
    StringValue_Index = 4,
    ResValue_Index = 10,
    LuaValue = t_1,
    ArrayValue = t_2,
    DicValue = t_5,
}
cofing_TestSheet_1[9].__index = cofing_TestSheet_1[9]

cofing_TestSheet_1[11] = {
    ID = 11,
    LongValue = 6,
    FloatValue = 1,
    StringValue_Index = 2,
    TextValue = 1,
    LuaValue = t_1,
    ArrayValue = t_2,
    DicValue = t_5,
}
cofing_TestSheet_1[11].__index = cofing_TestSheet_1[11]

cofing_TestSheet_1[12] = {
    ID = 12,
    LongValue = 1,
    FloatValue = 24.2,
    StringValue_Index = 5,
    ResValue_Index = 11,
    LuaValue = t_1,
    ArrayValue = t_2,
    DicValue = t_5,
}
cofing_TestSheet_1[12].__index = cofing_TestSheet_1[12]

cofing_TestSheet_1[14] = {
    ID = 14,
    FloatValue = 124.2,
    StringValue_Index = 6,
    ResValue_Index = 12,
    LuaValue = t_1,
    ArrayValue = t_2,
    DicValue = t_5,
}
cofing_TestSheet_1[14].__index = cofing_TestSheet_1[14]

cofing_TestSheet_1[20] = {
    ID = 20,
    LongValue = 2,
    FloatValue = 2,
    StringValue_Index = 1,
    ResValue_Index = 8,
    LuaValue = t_1,
    ArrayValue = t_2,
    DicValue = t_5,
}
cofing_TestSheet_1[20].__index = cofing_TestSheet_1[20]

cofing_TestSheet_1[31] = {
    ID = 31,
    LongValue = 754,
    LuaValue = t_1,
    ArrayValue = t_2,
    DicValue = t_5,
}
cofing_TestSheet_1[31].__index = cofing_TestSheet_1[31]

cofing_TestSheet_1[32] = {
    ID = 32,
    LongValue = 2,
    FloatValue = 1.3,
    BoolValue = false,
    StringValue_Index = 7,
    LuaValue = t_1,
    ArrayValue = t_2,
    DicValue = t_5,
}
cofing_TestSheet_1[32].__index = cofing_TestSheet_1[32]

cofing_TestSheet_1[54] = {
    ID = 54,
    LongValue = 4,
    FloatValue = 1.6,
    BoolValue = false,
    ResValue_Index = 13,
    LuaValue = t_1,
    ArrayValue = t_2,
    DicValue = t_5,
}
cofing_TestSheet_1[54].__index = cofing_TestSheet_1[54]

cofing_TestSheet_1[111] = {
    ID = 111,
    FloatValue = 1.5,
    BoolValue = false,
    ResValue_Index = 14,
    LuaValue = t_1,
    ArrayValue = t_3,
    DicValue = t_5,
}
cofing_TestSheet_1[111].__index = cofing_TestSheet_1[111]

return cofing_TestSheet_1
