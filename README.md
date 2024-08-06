# EEC2FHIR

將原本上傳EEC的CDAR2格式轉為FHIR的專案

## Contributors

firely sdk使用git submodule

所以clone完之後cd到目錄

```
git submodule update --init --recursive
```

或是

```
git submodule init
git submodule update
```

讓他更新子模組

模組階層如下:(firely 4.3.0)

```
├─ EEC2FHIR
  ├─ firely-net-sdk
    ├─ firel-net-common
```