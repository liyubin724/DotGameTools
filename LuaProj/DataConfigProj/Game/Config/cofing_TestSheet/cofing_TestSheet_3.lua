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
    [1] = 1,
    [2] = 2,
}
local cofing_TestSheet_3 = {}
cofing_TestSheet_3.__index = cofing_TestSheet_3
setmetatable(cofing_TestSheet_3,ConfigSubSheet)
cofing_TestSheet_3.ids = {12,14,20,}
cofing_TestSheet_3[12] = {
    ID = 12,
    LongValue = 1,
    FloatValue = 24.2,
    StringValue_Index = 5,
    ResValue_Index = 11,
    LuaValue = t_1,
    ArrayValue = t_2,
    DicValue = t_3,
}
cofing_TestSheet_3[12].__index = cofing_TestSheet_3[12]
setmetatable(cofing_TestSheet_3[12],ConfigSheetLine)

cofing_TestSheet_3[14] = {
    ID = 14,
    FloatValue = 124.2,
    StringValue_Index = 6,
    ResValue_Index = 12,
    LuaValue = t_1,
    ArrayValue = t_2,
    DicValue = t_3,
}
cofing_TestSheet_3[14].__index = cofing_TestSheet_3[14]
setmetatable(cofing_TestSheet_3[14],ConfigSheetLine)

cofing_TestSheet_3[20] = {
    ID = 20,
    LongValue = 2,
    FloatValue = 2,
    StringValue_Index = 1,
    ResValue_Index = 8,
    LuaValue = t_1,
    ArrayValue = t_2,
    DicValue = t_3,
}
cofing_TestSheet_3[20].__index = cofing_TestSheet_3[20]
setmetatable(cofing_TestSheet_3[20],ConfigSheetLine)

return cofing_TestSheet_3
