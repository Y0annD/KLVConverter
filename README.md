# KLVConverter

The aim of this library is to read SMPTE Standard KLV encoded stream.

## Reference document

This projets relates to following documents:

| Reference | Revision | Date | Name |Link|
|:---------:|:--------:|:----:|:----:|:--:|
| ST 336:2017 | 2017 | 2017 | SMPTE Standard Data Encoding Protocol Using Key-Length-Value | [ST0336:2017](https://pub.smpte.org/pub/st336/st0336-2017.pdf) |
| MISB ST0601| 19 | 23 October 2014 | MISB UAS Datalink Local Set | [MISB ST0601.19](https://nsgreg.nga.mil/doc/view?i=5471)
| MISB ST1201| 5 | 24 June 2021 | Floating Point to Integer Mapping | [MISB ST1201.5](https://nsgreg.nga.mil/doc/view?i=5276)

Most of the documents can be found on this portail: [NGA](https://nsgreg.nga.mil/misb.jsp)

# Release

## V1.0.1

- Add of IMAPB KLV To softawre reading
- Coverage test enhancement

# Documentation

The documentation __asciidoc__ sources are available in __doc__ directory.

To build the documentation, use the following commands:

```bash
chmod +x builddoc.sh
./builddoc.sh
```