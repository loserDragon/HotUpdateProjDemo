using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SUIFW {
    public class ABRelation {
        private string m_ABName;
        private List<string> m_ListABDependence;
        private List<string> m_ListABReference;


        public ABRelation(string ABName) {
            if (string.IsNullOrEmpty(ABName)) {
                Debug.LogError(GetType()+ "/构造方法/参数ABName不能为空，请检查！");
                return;
            }
            this.m_ABName = ABName;
            m_ListABDependence = new List<string>();
            m_ListABReference = new List<string>();
        }

        public void AddABDependence(string ABName) {
            if (string.IsNullOrEmpty(ABName)) {
                Debug.LogError(GetType()+ "/AddABDependence()/参数ABName不能为空，请检查！");
                return;
            }
            if (m_ListABDependence == null) {
                Debug.LogError(GetType() + "/AddABDependence()/m_ListABDependence不能为空，请检查！");
                return;
            }

            if (!m_ListABDependence.Contains(ABName) ) {
                m_ListABDependence.Add(ABName);
            }
        }

        public bool RemoveABDependence(string ABName) {
            if (string.IsNullOrEmpty(ABName) || m_ListABDependence == null) {
                Debug.LogError(GetType() + "/AddABDependence()/参数ABName不能为空或m_ListABDependence不能为空，请检查！");
                return false;
            }

            if (m_ListABDependence.Contains(ABName)) {
                m_ListABDependence.Remove(ABName);
            }

            return m_ListABDependence.Count > 0 ? false : true;
        }

        public List<string> GetAllABDependence() {
            return m_ListABDependence;
        }

        public void AddABReference(string ABName) {
            if (string.IsNullOrEmpty(ABName) || m_ListABReference == null) {
                Debug.LogError(GetType() + "/AddABDependence()/参数ABName不能为空或m_ListABReference不能为空，请检查！");
                return;
            }

            if (!m_ListABReference.Contains(ABName)) {
                m_ListABReference.Add(ABName);
            }
        }

        public bool RemoveABReference(string ABName) {
            if (string.IsNullOrEmpty(ABName) || m_ListABReference == null) {
                Debug.LogError(GetType() + "/AddABDependence()/参数ABName不能为空或m_ListABReference不能为空，请检查！");
                return false;
            }

            if (m_ListABReference.Contains(ABName)) {
                m_ListABReference.Remove(ABName);
            }

            return m_ListABReference.Count > 0 ? false : true;
        }

        public List<string> GetAllABReference() {
            return m_ListABReference;
        }
    }
}
