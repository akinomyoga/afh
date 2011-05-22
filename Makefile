# -*- Makefile -*-

tools: mwg.inc_ver

mwg.inc_ver: $(HOME)/bin/mwg.inc_ver
	cp -p $< $@
