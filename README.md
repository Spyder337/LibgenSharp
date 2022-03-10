# LibgenSharp

An interface for automating downloading books from LibGen using a single ISBN or providing a list of ISBNs from a file. File parsing is done with regex so ISBNs can be separated in any form the user sees fit.



### Usage

Example:

​	```dotnet LibgenSharp.exe -s <isbn> -ft <filetype>```

Search arguments always come first

Arguments

- -s Single search: pass in ISBN
- -fs File search: pass in file path

Optional Arguments

- -ft File type: preference with no . so "pdf" instead of ".pdf"



## TODO

- [x] Single ISBN search
- [x] Single Book download
- [x] Multi book download