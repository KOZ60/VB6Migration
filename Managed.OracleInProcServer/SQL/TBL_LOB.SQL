DROP TABLE TBL_LOB
/
CREATE TABLE TBL_LOB
(
    SEQ                            NUMBER NOT NULL,
    ITEM_BLOB                      BLOB,
    ITEM_CLOB                      CLOB,
    CONSTRAINT TBL_LOB_PKEY PRIMARY KEY (SEQ) USING INDEX
        ENABLE
)
/
