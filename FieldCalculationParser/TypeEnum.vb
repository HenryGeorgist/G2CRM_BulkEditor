<Flags>
Public Enum TypeEnum As Integer
    ERR = 0
    Dub = 1 << 0
    Sng = 1 << 1
    Deciml = Dub + Sng
    Shrt = 1 << 3
    Int = 1 << 4
    byt = 1 << 5
    integral = Shrt + Int + byt
    Num = Dub + Sng + Shrt + Int + byt
    Str = 1 << 6
    Bool = 1 << 7
    UnDeclared = 1 << 8
    Valid = Dub + Sng + Shrt + Int + byt + Str + Bool + UnDeclared
End Enum
