#!/bin/bash

if [[ "$1" == "compile" ]]
then

	# dlls
    dlls="-r:System -r:System.Windows.Forms -r:System.Drawing"

    # .cs files
    mysrc=""
    for f in ./*.cs
    do
        mysrc+=" $f"
    done

	if [[ "$2" == "" ]]
	then
		mcs -out:TuringSimulator.exe $mysrc $dlls
	fi	

	if [[ "$2" == "run" ]]
	then
		mcs -out:TuringSimulator.exe $mysrc $dlls && mono ./TuringSimulator.exe
	fi
fi


if [[ "$1" == "help" ]]
then
	echo "compile or compile run"
fi