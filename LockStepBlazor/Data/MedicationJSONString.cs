using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Hl7.FhirPath.Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace LockStepBlazor.Data
{
    public static class MedicationJSONString
    {
        public static string MedicationString { get; set; } =
        #region Fhir Response string

         @"{
  'resourceType': 'Bundle',
  'id': '4cf2ec7b-cd3e-4c0c-ad7b-a75e17b85867',
  'type': 'batch-response',
  'link': [ {
    'relation': 'self',
    'url': 'http://hapi.fhir.org/baseR4'
  } ],
  'entry': [ {
    'resource': {
      'resourceType': 'Bundle',
      'id': '055ddcb0-fb8d-48c6-b27c-5bbed30b1f1b',
      'meta': {
        'lastUpdated': '2020-05-05T20:43:06.906+00:00'
      },
      'type': 'searchset',
      'total': 25,
      'link': [ {
        'relation': 'self',
        'url': 'http://hapi.fhir.org/baseR4/MedicationRequest?_count=1000&_include=MedicationRequest%3Amedication&patient=921330'
      } ],
      'entry': [ {
        'fullUrl': 'http://hapi.fhir.org/baseR4/MedicationRequest/1137702',
        'resource': {
          'resourceType': 'MedicationRequest',
          'id': '1137702',
          'meta': {
            'versionId': '2',
            'lastUpdated': '2020-04-28T01:41:58.124+00:00',
            'source': '#0tmp4bC3YyTPS3n9'
          },
          'status': 'active',
          'intent': 'original-order',
          'medicationCodeableConcept': {
            'coding': [ {
              'system': 'http://hl7.org/fhir/sid/ndc',
              'code': '0113-7131-06'
            } ],
            'text': 'BASIC CARE IBUPROFEN 1 BOTTLE in 1 CARTON > 160 CAPSULE, LIQUID FILLED in 1 BOTTLE'
          },
          'subject': {
            'reference': 'Patient/921330'
          }
        },
        'search': {
          'mode': 'match'
        }
      }, {
        'fullUrl': 'http://hapi.fhir.org/baseR4/MedicationRequest/1137701',
        'resource': {
          'resourceType': 'MedicationRequest',
          'id': '1137701',
          'meta': {
            'versionId': '2',
            'lastUpdated': '2020-04-28T01:40:37.044+00:00',
            'source': '#G5ZijCqBgBgcbjaa'
          },
          'status': 'active',
          'intent': 'original-order',
          'medicationCodeableConcept': {
            'coding': [ {
              'system': 'http://hl7.org/fhir/sid/ndc',
              'code': '49580-1434-4'
            } ],
            'text': 'Childrens Plus Multi-symptom Cold Grape With Pe'
          },
          'subject': {
            'reference': 'Patient/921330'
          }
        },
        'search': {
          'mode': 'match'
        }
      }, {
        'fullUrl': 'http://hapi.fhir.org/baseR4/MedicationRequest/1137700',
        'resource': {
          'resourceType': 'MedicationRequest',
          'id': '1137700',
          'meta': {
            'versionId': '2',
            'lastUpdated': '2020-04-28T01:39:12.000+00:00',
            'source': '#9fTgW6fgDybynKoA'
          },
          'status': 'active',
          'intent': 'original-order',
          'medicationCodeableConcept': {
            'coding': [ {
              'system': 'http://hl7.org/fhir/sid/ndc',
              'code': '67618-101-10'
            } ],
            'text': 'COLACE 10 Capsule'
          },
          'subject': {
            'reference': 'Patient/921330'
          }
        },
        'search': {
          'mode': 'match'
        }
      }, {
        'fullUrl': 'http://hapi.fhir.org/baseR4/MedicationRequest/1137699',
        'resource': {
          'resourceType': 'MedicationRequest',
          'id': '1137699',
          'meta': {
            'versionId': '2',
            'lastUpdated': '2020-04-28T01:37:29.979+00:00',
            'source': '#sbxnyoBhyzRzmszg'
          },
          'status': 'active',
          'intent': 'original-order',
          'medicationCodeableConcept': {
            'coding': [ {
              'system': 'http://hl7.org/fhir/sid/ndc',
              'code': '0093-7867-65'
            } ],
            'text': 'FENTANYL CITRATE lozenge'
          },
          'subject': {
            'reference': 'Patient/921330'
          }
        },
        'search': {
          'mode': 'match'
        }
      }, {
        'fullUrl': 'http://hapi.fhir.org/baseR4/MedicationRequest/1137698',
        'resource': {
          'resourceType': 'MedicationRequest',
          'id': '1137698',
          'meta': {
            'versionId': '2',
            'lastUpdated': '2020-04-28T01:30:55.750+00:00',
            'source': '#WmHRK2e5UkP7ICdT'
          },
          'status': 'active',
          'intent': 'original-order',
          'medicationCodeableConcept': {
            'coding': [ {
              'system': 'http://hl7.org/fhir/sid/ndc',
              'code': '0078-0675-15'
            } ],
            'text': 'Zofran, 30 tabs'
          },
          'subject': {
            'reference': 'Patient/921330'
          }
        },
        'search': {
          'mode': 'match'
        }
      }, {
        'fullUrl': 'http://hapi.fhir.org/baseR4/MedicationRequest/1094527',
        'resource': {
          'resourceType': 'MedicationRequest',
          'id': '1094527',
          'meta': {
            'versionId': '2',
            'lastUpdated': '2020-04-05T16:45:12.079+00:00',
            'source': '#ACK67413QMTWdyiC'
          },
          'status': 'active',
          'intent': 'original-order',
          'medicationCodeableConcept': {
            'coding': [ {
              'system': 'http://snomed.info/sct',
              'code': '430127000'
            } ],
            'text': 'Oral Form Oxycodone (product)'
          },
          'subject': {
            'reference': 'Patient/921330'
          }
        },
        'search': {
          'mode': 'match'
        }
      }, {
        'fullUrl': 'http://hapi.fhir.org/baseR4/MedicationRequest/1094526',
        'resource': {
          'resourceType': 'MedicationRequest',
          'id': '1094526',
          'meta': {
            'versionId': '2',
            'lastUpdated': '2020-04-05T16:45:11.125+00:00',
            'source': '#ptbH8psFW8Gx8dXM'
          },
          'status': 'active',
          'intent': 'original-order',
          'medicationCodeableConcept': {
            'coding': [ {
              'system': 'http://www.nlm.nih.gov/research/umls/rxnorm',
              'code': '330765'
            } ],
            'text': 'rizatriptan 10 MG'
          },
          'subject': {
            'reference': 'Patient/921330'
          }
        },
        'search': {
          'mode': 'match'
        }
      }, {
        'fullUrl': 'http://hapi.fhir.org/baseR4/MedicationRequest/1094525',
        'resource': {
          'resourceType': 'MedicationRequest',
          'id': '1094525',
          'meta': {
            'versionId': '2',
            'lastUpdated': '2020-04-05T16:45:10.217+00:00',
            'source': '#HKrQPB6ZNzbLF25F'
          },
          'status': 'active',
          'intent': 'original-order',
          'medicationCodeableConcept': {
            'coding': [ {
              'system': 'http://www.nlm.nih.gov/research/umls/rxnorm',
              'code': '608930'
            } ],
            'text': 'Urokinase 50000 UNT/ML Injectable Solution'
          },
          'subject': {
            'reference': 'Patient/921330'
          }
        },
        'search': {
          'mode': 'match'
        }
      }, {
        'fullUrl': 'http://hapi.fhir.org/baseR4/MedicationRequest/1094524',
        'resource': {
          'resourceType': 'MedicationRequest',
          'id': '1094524',
          'meta': {
            'versionId': '2',
            'lastUpdated': '2020-04-05T16:45:03.399+00:00',
            'source': '#vygcbiFwmd6I9Jqz'
          },
          'status': 'active',
          'intent': 'original-order',
          'medicationCodeableConcept': {
            'coding': [ {
              'system': 'http://www.nlm.nih.gov/research/umls/rxnorm',
              'code': '141962'
            } ],
            'text': 'Azithromycin 250 MG Oral Capsule'
          },
          'subject': {
            'reference': 'Patient/921330'
          }
        },
        'search': {
          'mode': 'match'
        }
      }, {
        'fullUrl': 'http://hapi.fhir.org/baseR4/MedicationRequest/1094444',
        'resource': {
          'resourceType': 'MedicationRequest',
          'id': '1094444',
          'meta': {
            'versionId': '2',
            'lastUpdated': '2020-04-04T19:28:41.974+00:00',
            'source': '#l0vh9Y31LkmirpBd'
          },
          'status': 'active',
          'intent': 'original-order',
          'medicationCodeableConcept': {
            'coding': [ {
              'system': 'http://hl7.org/fhir/sid/ndc',
              'code': '0338-1134-03'
            } ],
            'text': 'Clinimix 4.25 / 10 sulfite - free(4.25 % Amino Acid in 10 % Dextrose) Injection, 1000ml'
          },
          'subject': {
            'reference': 'Patient/921330'
          }
        },
        'search': {
          'mode': 'match'
        }
      }, {
        'fullUrl': 'http://hapi.fhir.org/baseR4/MedicationRequest/1093795',
        'resource': {
          'resourceType': 'MedicationRequest',
          'id': '1093795',
          'meta': {
            'versionId': '2',
            'lastUpdated': '2020-04-02T22:38:43.545+00:00',
            'source': '#85sgk9ImSDqANJgW'
          },
          'subject': {
            'reference': 'Patient/921330'
          }
        },
        'search': {
          'mode': 'match'
        }
      }, {
        'fullUrl': 'http://hapi.fhir.org/baseR4/MedicationRequest/1093793',
        'resource': {
          'resourceType': 'MedicationRequest',
          'id': '1093793',
          'meta': {
            'versionId': '2',
            'lastUpdated': '2020-04-02T22:38:15.181+00:00',
            'source': '#tboyi999FC6Y2cxN'
          },
          'subject': {
            'reference': 'Patient/921330'
          }
        },
        'search': {
          'mode': 'match'
        }
      }, {
        'fullUrl': 'http://hapi.fhir.org/baseR4/MedicationRequest/1093792',
        'resource': {
          'resourceType': 'MedicationRequest',
          'id': '1093792',
          'meta': {
            'versionId': '2',
            'lastUpdated': '2020-04-02T22:36:52.534+00:00',
            'source': '#14AO8XSyPlYauOrJ'
          },
          'subject': {
            'reference': 'Patient/921330'
          }
        },
        'search': {
          'mode': 'match'
        }
      }, {
        'fullUrl': 'http://hapi.fhir.org/baseR4/MedicationRequest/1093785',
        'resource': {
          'resourceType': 'MedicationRequest',
          'id': '1093785',
          'meta': {
            'versionId': '2',
            'lastUpdated': '2020-04-02T22:35:01.337+00:00',
            'source': '#17IoTBfIjZEHNhz0'
          },
          'subject': {
            'reference': 'Patient/921330'
          }
        },
        'search': {
          'mode': 'match'
        }
      }, {
        'fullUrl': 'http://hapi.fhir.org/baseR4/MedicationRequest/1093765',
        'resource': {
          'resourceType': 'MedicationRequest',
          'id': '1093765',
          'meta': {
            'versionId': '2',
            'lastUpdated': '2020-04-02T21:33:01.130+00:00',
            'source': '#AJJ7Tr5bE3KfJKtw'
          },
          'subject': {
            'reference': 'Patient/921330'
          }
        },
        'search': {
          'mode': 'match'
        }
      }, {
        'fullUrl': 'http://hapi.fhir.org/baseR4/MedicationRequest/1093764',
        'resource': {
          'resourceType': 'MedicationRequest',
          'id': '1093764',
          'meta': {
            'versionId': '2',
            'lastUpdated': '2020-04-02T21:31:47.204+00:00',
            'source': '#cwC9lTizoE2zoW6t'
          },
          'subject': {
            'reference': 'Patient/921330'
          }
        },
        'search': {
          'mode': 'match'
        }
      }, {
        'fullUrl': 'http://hapi.fhir.org/baseR4/MedicationRequest/1093763',
        'resource': {
          'resourceType': 'MedicationRequest',
          'id': '1093763',
          'meta': {
            'versionId': '2',
            'lastUpdated': '2020-04-02T21:31:26.874+00:00',
            'source': '#oBRlRASt61Gwv7o4'
          },
          'subject': {
            'reference': 'Patient/921330'
          }
        },
        'search': {
          'mode': 'match'
        }
      }, {
        'fullUrl': 'http://hapi.fhir.org/baseR4/MedicationRequest/1093762',
        'resource': {
          'resourceType': 'MedicationRequest',
          'id': '1093762',
          'meta': {
            'versionId': '2',
            'lastUpdated': '2020-04-02T21:25:46.600+00:00',
            'source': '#WCnGKqEEpnkaS39m'
          },
          'subject': {
            'reference': 'Patient/921330'
          }
        },
        'search': {
          'mode': 'match'
        }
      }, {
        'fullUrl': 'http://hapi.fhir.org/baseR4/MedicationRequest/1093761',
        'resource': {
          'resourceType': 'MedicationRequest',
          'id': '1093761',
          'meta': {
            'versionId': '2',
            'lastUpdated': '2020-04-02T21:25:07.201+00:00',
            'source': '#5lrHeFqaH2TUWdRO'
          },
          'subject': {
            'reference': 'Patient/921330'
          }
        },
        'search': {
          'mode': 'match'
        }
      }, {
        'fullUrl': 'http://hapi.fhir.org/baseR4/MedicationRequest/1091964',
        'resource': {
          'resourceType': 'MedicationRequest',
          'id': '1091964',
          'meta': {
            'versionId': '2',
            'lastUpdated': '2020-03-30T17:58:30.800+00:00',
            'source': '#5BAnSjt2rmVMclES'
          },
          'subject': {
            'reference': 'Patient/921330'
          }
        },
        'search': {
          'mode': 'match'
        }
      }, {
        'fullUrl': 'http://hapi.fhir.org/baseR4/MedicationRequest/1090735',
        'resource': {
          'resourceType': 'MedicationRequest',
          'id': '1090735',
          'meta': {
            'versionId': '2',
            'lastUpdated': '2020-03-28T05:11:55.774+00:00',
            'source': '#iCnkiNCAt4oeMWZg'
          },
          'subject': {
            'reference': 'Patient/921330'
          }
        },
        'search': {
          'mode': 'match'
        }
      }, {
        'fullUrl': 'http://hapi.fhir.org/baseR4/MedicationRequest/1090734',
        'resource': {
          'resourceType': 'MedicationRequest',
          'id': '1090734',
          'meta': {
            'versionId': '2',
            'lastUpdated': '2020-03-28T05:09:31.492+00:00',
            'source': '#7Z1K5kv8CUBGuJSM'
          },
          'subject': {
            'reference': 'Patient/921330'
          }
        },
        'search': {
          'mode': 'match'
        }
      }, {
        'fullUrl': 'http://hapi.fhir.org/baseR4/MedicationRequest/1090732',
        'resource': {
          'resourceType': 'MedicationRequest',
          'id': '1090732',
          'meta': {
            'versionId': '2',
            'lastUpdated': '2020-03-28T02:55:51.560+00:00',
            'source': '#xbvX2sOaxNJnC9yf'
          },
          'subject': {
            'reference': 'Patient/921330'
          }
        },
        'search': {
          'mode': 'match'
        }
      }, {
        'fullUrl': 'http://hapi.fhir.org/baseR4/MedicationRequest/1090731',
        'resource': {
          'resourceType': 'MedicationRequest',
          'id': '1090731',
          'meta': {
            'versionId': '2',
            'lastUpdated': '2020-03-28T02:52:33.211+00:00',
            'source': '#BZr9AKqISw6Kad0j'
          },
          'subject': {
            'reference': 'Patient/921330'
          }
        },
        'search': {
          'mode': 'match'
        }
      }, {
        'fullUrl': 'http://hapi.fhir.org/baseR4/MedicationRequest/921351',
        'resource': {
          'resourceType': 'MedicationRequest',
          'id': '921351',
          'meta': {
            'versionId': '2',
            'lastUpdated': '2020-03-26T03:26:09.887+00:00',
            'source': '#o7viDBubiZVu3iYG'
          },
          'subject': {
            'reference': 'Patient/921330'
          }
        },
        'search': {
          'mode': 'match'
        }
      } ]
    },
    'response': {
      'status': '200 OK'
    }
  }, {
    'resource': {
      'resourceType': 'Bundle',
      'id': 'ab40e025-b674-48f7-b548-8f5af22f3b12',
      'meta': {
        'lastUpdated': '2020-05-05T20:43:07.034+00:00'
      },
      'type': 'searchset',
      'total': 1,
      'link': [ {
        'relation': 'self',
        'url': 'http://hapi.fhir.org/baseR4/MedicationStatement?_count=1000&_include=MedicationStatement%3Amedication&patient=921330'
      } ],
      'entry': [ {
        'fullUrl': 'http://hapi.fhir.org/baseR4/MedicationStatement/1148019',
        'resource': {
          'resourceType': 'MedicationStatement',
          'id': '1148019',
          'meta': {
            'versionId': '1',
            'lastUpdated': '2020-05-05T02:38:42.805+00:00',
            'source': '#TgOqmlZb1cfCaULX'
          },
          'status': 'active',
          'medicationCodeableConcept': {
            'coding': [ {
              'system': 'http://hl7.org/fhir/sid/ndc',
              'code': '50580-600-02'
            } ],
            'text': 'TYLENOL REGULAR STRENGTH'
          },
          'subject': {
            'reference': 'Patient/921330'
          },
          'effectiveDateTime': '2020-04-26T21:38:35.2277443-05:00'
        },
        'search': {
          'mode': 'match'
        }
      } ]
    },
    'response': {
      'status': '200 OK'
    }
  } ]
}";
        #endregion
        
        public static Task<Bundle> ParseMedsAsync()
        {
            var parser = new FhirJsonParser();
          
             return Task.Run(() =>  { return parser.Parse<Bundle>(MedicationString); } );
            
           
        }

    }

    
}
