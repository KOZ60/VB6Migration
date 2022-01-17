DROP TABLE TBL_ALLTYPES
/
CREATE TABLE TBL_ALLTYPES
(
    SEQ                            NUMBER NOT NULL,
    ITEM_VARCHAR2                  VARCHAR2(10),
    ITEM_CHAR                      CHAR(10),
    ITEM_DATE                      DATE,
    ITEM_INT16                     NUMBER(4,0),
    ITEM_INT32                     NUMBER(10,0),
    ITEM_INT64                     NUMBER(19,0),
    ITEM_DECIMAL                   NUMBER(24,5),
    ITEM_RAW                       RAW(50),
    ITEM_TIMESTAMP                 TIMESTAMP(6),
    ITEM_TIMESTAMPLTZ              TIMESTAMP(6) WITH LOCAL TIME ZONE,
    ITEM_TIMESTAMPTZ               TIMESTAMP(6) WITH TIME ZONE,
    ITEM_INTERVALDS                INTERVAL DAY(2) TO SECOND(6),
    ITEM_INTERVALYM                INTERVAL YEAR(2) TO MONTH,
    ITEM_NUMBER                    NUMBER(10,0),
    ITEM_SINGLE                    FLOAT(126),
    ITEM_DOUBLE                    BINARY_DOUBLE,
    ITEM_DECIMAL2                  NUMBER,
    ITEM_BFILE                     BFILE,
    ITEM_BLOB                      BLOB,
    ITEM_CLOB                      CLOB,
    ITEM_LONG                      LONG,
    CONSTRAINT ALLTYPES_PK PRIMARY KEY (SEQ) USING INDEX
        ENABLE
)
/
