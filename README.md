# RangeCalendarCSharp

This creates the famous functional calendar page in C#.

It started as a simple port but ended in adding more and more features and influencing the [Funcky](https://github.com/polyadic/funcky/) functional library.

This calendar handles a lot of the cultural differences between calendars
* it correctly handles start of week
* we localize the months and weekdays 
* we can display weekend and holidays by country

## The C++ range-v3 implementation by Eric Niebler

This talk was inspired by the [Component programming with ranges](https://wiki.dlang.org/Component_programming_with_ranges) on the D language wiki.


* https://www.youtube.com/watch?v=mFUXNMfaciE
* https://github.com/ericniebler/range-v3/blob/master/example/calendar.cpp

## An implementation in Haskell

* https://github.com/BartoszMilewski/Calendar/blob/master/Main.hs

## An implementation in Rust
* https://play.rust-lang.org/?gist=1057364daeee4cff472a&version=nightly

## An implementation with rangeless in C++

* https://github.com/ast-al/rangeless/blob/gh-pages/test/calendar.cpp

## Features

* Accepts a number, this is interpreted as the `year` of the calendar
* Accepts Cultures as `en-GB` or `hu-HU` to localize the calendar correctly (no invariant cultures)
* Add the parameter `stream` to have an endless stream starting with year
* Add the parameter `fancy` to colorize the output, weekends are red, holidays have a redish backround.

## The functional magic

The code is very generic and has almost no code to treat special cases. The code has 264 Lines of Codes of which 72 are executable. No function has cyclomatic complexity of more than 2!

* It's mostly pure functional with a little global state which you can define via the console. (You could certainly hide that better)
* The .NET CultureInfo classes give information on dates: weekday, month, weeknumber and also when a week starts.
* The .NET CultureInfo classes also have the localized names for months and week days
* For Weekend and Holiday calculation we use the library [Nager.Date](https://github.com/nager/Nager.Date)

## The current result of the Program

### en-GB

```
     January 2020         February 2020           March 2020
 Su Mo Tu We Th Fr Sa  Su Mo Tu We Th Fr Sa  Su Mo Tu We Th Fr Sa
           1  2  3  4                     1   1  2  3  4  5  6  7
  5  6  7  8  9 10 11   2  3  4  5  6  7  8   8  9 10 11 12 13 14
 12 13 14 15 16 17 18   9 10 11 12 13 14 15  15 16 17 18 19 20 21
 19 20 21 22 23 24 25  16 17 18 19 20 21 22  22 23 24 25 26 27 28
 26 27 28 29 30 31     23 24 25 26 27 28 29  29 30 31

      April 2020             May 2020             June 2020
 Su Mo Tu We Th Fr Sa  Su Mo Tu We Th Fr Sa  Su Mo Tu We Th Fr Sa
           1  2  3  4                  1  2      1  2  3  4  5  6
  5  6  7  8  9 10 11   3  4  5  6  7  8  9   7  8  9 10 11 12 13
 12 13 14 15 16 17 18  10 11 12 13 14 15 16  14 15 16 17 18 19 20
 19 20 21 22 23 24 25  17 18 19 20 21 22 23  21 22 23 24 25 26 27
 26 27 28 29 30        24 25 26 27 28 29 30  28 29 30
                       31

      July 2020            August 2020          September 2020
 Su Mo Tu We Th Fr Sa  Su Mo Tu We Th Fr Sa  Su Mo Tu We Th Fr Sa
           1  2  3  4                     1         1  2  3  4  5
  5  6  7  8  9 10 11   2  3  4  5  6  7  8   6  7  8  9 10 11 12
 12 13 14 15 16 17 18   9 10 11 12 13 14 15  13 14 15 16 17 18 19
 19 20 21 22 23 24 25  16 17 18 19 20 21 22  20 21 22 23 24 25 26
 26 27 28 29 30 31     23 24 25 26 27 28 29  27 28 29 30
                       30 31

     October 2020         November 2020         December 2020
 Su Mo Tu We Th Fr Sa  Su Mo Tu We Th Fr Sa  Su Mo Tu We Th Fr Sa
              1  2  3   1  2  3  4  5  6  7         1  2  3  4  5
  4  5  6  7  8  9 10   8  9 10 11 12 13 14   6  7  8  9 10 11 12
 11 12 13 14 15 16 17  15 16 17 18 19 20 21  13 14 15 16 17 18 19
 18 19 20 21 22 23 24  22 23 24 25 26 27 28  20 21 22 23 24 25 26
 25 26 27 28 29 30 31  29 30                 27 28 29 30 31
```

### 2021 de-CH

```
        Januar               Februar                 März
 Mo Di Mi Do Fr Sa So  Mo Di Mi Do Fr Sa So  Mo Di Mi Do Fr Sa So
              1  2  3   1  2  3  4  5  6  7   1  2  3  4  5  6  7
  4  5  6  7  8  9 10   8  9 10 11 12 13 14   8  9 10 11 12 13 14
 11 12 13 14 15 16 17  15 16 17 18 19 20 21  15 16 17 18 19 20 21
 18 19 20 21 22 23 24  22 23 24 25 26 27 28  22 23 24 25 26 27 28
 25 26 27 28 29 30 31                                    29 30 31

        April                  Mai                   Juni
 Mo Di Mi Do Fr Sa So  Mo Di Mi Do Fr Sa So  Mo Di Mi Do Fr Sa So
           1  2  3  4                  1  2      1  2  3  4  5  6
  5  6  7  8  9 10 11   3  4  5  6  7  8  9   7  8  9 10 11 12 13
 12 13 14 15 16 17 18  10 11 12 13 14 15 16  14 15 16 17 18 19 20
 19 20 21 22 23 24 25  17 18 19 20 21 22 23  21 22 23 24 25 26 27
       26 27 28 29 30  24 25 26 27 28 29 30              28 29 30
                                         31

         Juli                 August              September
 Mo Di Mi Do Fr Sa So  Mo Di Mi Do Fr Sa So  Mo Di Mi Do Fr Sa So
           1  2  3  4                     1         1  2  3  4  5
  5  6  7  8  9 10 11   2  3  4  5  6  7  8   6  7  8  9 10 11 12
 12 13 14 15 16 17 18   9 10 11 12 13 14 15  13 14 15 16 17 18 19
 19 20 21 22 23 24 25  16 17 18 19 20 21 22  20 21 22 23 24 25 26
    26 27 28 29 30 31  23 24 25 26 27 28 29           27 28 29 30
                                      30 31

       Oktober               November              Dezember
 Mo Di Mi Do Fr Sa So  Mo Di Mi Do Fr Sa So  Mo Di Mi Do Fr Sa So
              1  2  3   1  2  3  4  5  6  7         1  2  3  4  5
  4  5  6  7  8  9 10   8  9 10 11 12 13 14   6  7  8  9 10 11 12
 11 12 13 14 15 16 17  15 16 17 18 19 20 21  13 14 15 16 17 18 19
 18 19 20 21 22 23 24  22 23 24 25 26 27 28  20 21 22 23 24 25 26
 25 26 27 28 29 30 31                 29 30        27 28 29 30 31
```

### 2000 ru-RU

```
     Январь 2020           Февраль 2020           Март 2020
 Пн Вт Ср Чт Пт Сб Вс  Пн Вт Ср Чт Пт Сб Вс  Пн Вт Ср Чт Пт Сб Вс
                 1  2      1  2  3  4  5  6         1  2  3  4  5
  3  4  5  6  7  8  9   7  8  9 10 11 12 13   6  7  8  9 10 11 12
 10 11 12 13 14 15 16  14 15 16 17 18 19 20  13 14 15 16 17 18 19
 17 18 19 20 21 22 23  21 22 23 24 25 26 27  20 21 22 23 24 25 26
 24 25 26 27 28 29 30                 28 29        27 28 29 30 31
                   31

    Апрель 2020             Май 2020             Июнь 2020
 Пн Вт Ср Чт Пт Сб Вс  Пн Вт Ср Чт Пт Сб Вс  Пн Вт Ср Чт Пт Сб Вс
                 1  2   1  2  3  4  5  6  7            1  2  3  4
  3  4  5  6  7  8  9   8  9 10 11 12 13 14   5  6  7  8  9 10 11
 10 11 12 13 14 15 16  15 16 17 18 19 20 21  12 13 14 15 16 17 18
 17 18 19 20 21 22 23  22 23 24 25 26 27 28  19 20 21 22 23 24 25
 24 25 26 27 28 29 30              29 30 31        26 27 28 29 30

      Июль 2020            Август 2020          Сентябрь 2020
 Пн Вт Ср Чт Пт Сб Вс  Пн Вт Ср Чт Пт Сб Вс  Пн Вт Ср Чт Пт Сб Вс
                 1  2      1  2  3  4  5  6               1  2  3
  3  4  5  6  7  8  9   7  8  9 10 11 12 13   4  5  6  7  8  9 10
 10 11 12 13 14 15 16  14 15 16 17 18 19 20  11 12 13 14 15 16 17
 17 18 19 20 21 22 23  21 22 23 24 25 26 27  18 19 20 21 22 23 24
 24 25 26 27 28 29 30           28 29 30 31     25 26 27 28 29 30
                   31

    Октябрь 2020          Ноябрь 2020           Декабрь 2020
 Пн Вт Ср Чт Пт Сб Вс  Пн Вт Ср Чт Пт Сб Вс  Пн Вт Ср Чт Пт Сб Вс
                    1         1  2  3  4  5               1  2  3
  2  3  4  5  6  7  8   6  7  8  9 10 11 12   4  5  6  7  8  9 10
  9 10 11 12 13 14 15  13 14 15 16 17 18 19  11 12 13 14 15 16 17
 16 17 18 19 20 21 22  20 21 22 23 24 25 26  18 19 20 21 22 23 24
 23 24 25 26 27 28 29           27 28 29 30  25 26 27 28 29 30 31
                30 31
```
