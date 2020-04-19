### Deividas Vaičiūnas IIF 16/1

### Students management system on ASP.Net Core (C#)

# Turinys

- Versijos ir jų pakeitimai
    - [v0.1](#v0.1)
    - [v0.2](#v0.2)
    - [v0.3](#v0.3)
    - [v0.4](#v0.4)
    - [v0.5](#v0.5)
        - [Rūšiavimo palyginimas](#rūšiavimo-palyginimas)
            - LinkedList
            - List
            - Queue
    - [v1.0 (Final release)](#v1.0)

# Versijos ir jų pakeitimai

## v0.1

[Grįžti į turinį](#turinys)

Pirmoji iteracija kuriant studentų valdymo sistemą.

Šios iteracijos metu sukurta:

1. Vartotojo sąsaja
2. Studentų bei jų pasiekimų valdymas
3. Duomenų atvaizdavimas

Programa valdoma StudentsManager.cs klasėje.

```c#
// Konstruktorius
public StudentsManager()
{
    Working = true;
    VisibleContent = VisibleContent.MainMenu;
    Students = new List<Student>();
}
```

```c#
// Veikimo variklis. Kol Working == true, tol jungiamas vienas iš pasirinktų langų
// Pirmiausia suteikiama VisibleContent.MainMenu reikšmė.
public void Run()
{
    while (Working)
    {
        try
        {
            switch (VisibleContent)
            {
                case VisibleContent.MainMenu:
                    MainMenuView();
                    break;
                case VisibleContent.AddStudent:
                    AddStudentView();
                    break;
                case VisibleContent.AddGradesHomework:
                    AddHomeworkGradesView();
                    break;
                case VisibleContent.AddGradesExam:
                    AddExamGradesView();
                    break;
                case VisibleContent.ListOfStudents:
                    ListOfStudentsView(true);
                    break;
                case VisibleContent.RemoveStudent:
                    RemoveStudentView();
                    break;
                default:
                    break;
            }
        }
    }
}
```

```c#
// Pasirenkimai meniu valdomi įvedamu norimo lango skaičiu

Console.WriteLine($"Write a number and press enter to change the view\n");
Console.WriteLine($"1. Add student(-s)");
Console.WriteLine($"2. Add homework grades for a student");
Console.WriteLine($"3. Add an exam grade for a student");
Console.WriteLine($"4. List students with their averages");
Console.WriteLine($"5. Remove student(-s)");
Console.WriteLine($"6. Import students from file");
Console.WriteLine($"7. Generate students");
Console.WriteLine($"0. Exit");
```

Studentai bei jų pasiekimai saugomi objektiškai - Student.cs klasės pagalba.

<br />

## v0.2

[Grįžti į turinį](#turinys)

Studentų nuskaitymas ir generavimas iš *.txt* dokumento.

Dokumento struktūra:

```txt
Vardas		    Pavarde			ND1	ND2	ND3	ND4	ND5	Egzaminas
Jonas		    Jonaitis		5	10	6	9	9	7
Ramunas		    Tikrairamus		8	9	10	10	9	9
Arvydas		    Saboniukas		8	9	10	6	10	9
Mantas		    Kalnietis		6	6	7	9	10	8
```

Nuskaitymui sukurtas naujas meniu punktas:

```c#
// pasirenkamas i6 meniu numeriu 6

case VisibleContent.StudentImport:
    ImportStudentsView();
    break;
```

Dokumentas būtinai turi būti patalpintas projekto viduje: *../data* aplankale *students.txt* pavadinimu.

Esant klaidai metamas *Exception* tipo pranešimas ir esate grąžinami į meniu langą.
Jei įvyksta klaida nuskaitant duomenis iš dokumento - klausiama, ar norime tęsti ir praleisti nenuskaitomą dokumento dalį, ar nutraukti visą procesą.

<br />

## v0.3

[Grįžti į turinį](#turinys)

Visoje programoje naudojami *try - catch* blokai su apdorojimu prieš grįžtant į meniu langą

```c#
try 
{
    // code
} 
catch(Exception e) 
{
    // exception handling
} 
finally 
{
    // set view to main menu
}
```

Meniu dalyje apdorojimas vyksta išvedant *InnerStackTrace* pranešimą, *Message* ir parodant viską, kas talpinama *Exception.Data* parametre (*Key, Value*).

<br />

## v0.4

[Grįžti į turinį](#turinys)

Sukurtas studentų bei jų pasiekimų generavimas atsitiktine tvarka naudojant šabloninius vardus bei pavardes, pvz: *Vardas1 Pavardė1, Vardas2 Pavardė2 ir t.t.*

Sukurtas naujas meniu punktas - 7. Šiuo meniu pasirinkimu esate nukeliamas į generavimo langą, kuriame pasirenkate vieną iš penkių pasirinkimų - kiek šabloninių studentų sugeneruoti.

Tuo pačiu - sugeneravus studentų dokumentą jis yra išskaidomas į du papildomus dokumentus *Vargsiukai.txt ir Saunuoliai.txt*, kur *Vargsiukai.txt* dokumente saugomi studentai, kurių bendras vidurkis yra mažesnis negu 5, o *Saunuoliai.txt* saugomi studentai, kurių bendras vidurkis didesnis arba lygus 5.

<br />

## v0.5

### Rūšiavimo palyginimas

[Grįžti į turinį](#turinys)

Generuojamas *Students.txt* dokumentas naudojant 3 skirtingus konteinerių tipus bei pasitelkiant kiekvieno iš jų rūšiavimo galimybėmis: *LinkedList, List ir Queue*.

Turimas sugeneruotas *GeneratedStudents{count}.txt* dokumentas nuskaitomas, surūšiuojams ir talpinamas į atitinkamus dokumentus.

Visi testai buvo atliekami rūšiuojant tą patį dokumentą (1 000 000 studentų).

- LinkedList: užtruko 18.6465553 s.
- List: užtruko 19.4243362 s.
- Queue: užtruko 19.3866139 s.

Išvada: greičiausiai veikia LinkedList rūšiavimas.

<br />

## v1.0

[Grįžti į turinį](#turinys)

Generuojami studentai surūšiuojami ir išskaidomi pagal jų rezultatus.

Pasirinkus 7 meniu punktą ir pasirinkus studentų kiekį klausiama: *Ar norime, kad pilnas GeneratedStudents.txt dokumentas būtų surūšiuotas ir išsskaidytas (Y/N), kur Y reiškia taip, o N arba bet kas - ne*.

Pradedamas generavimas pilno dokumento, paskui pradedamas skaityti ir generuojami išskaidyti dokumentai naudojant 3 skirtingus konteinerius: *LinkeList, List ir Queue*.

Konteineriai surašyti nuo greičiausiai atlikusio užduotį iki lėčiausiai (1 000 000 studentų):

- LinkedList *19.2 s.*
- Queue *19.6 s.*
- List *21.3 s.*