DROP TABLE TBL_ALLTYPES2
/
CREATE TABLE TBL_ALLTYPES2
(
    SEQ                            NUMBER NOT NULL,
    ITEM_LONGRAW                   LONG RAW,
    ITEM_ORABFILE                  BFILE,
    ITEM_ORABFILE2                 BFILE,
    CONSTRAINT ALLTYPES2_PK PRIMARY KEY (SEQ) USING INDEX
        ENABLE
)
/
