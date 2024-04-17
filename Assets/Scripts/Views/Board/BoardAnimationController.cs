using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Hex.Views
{
    public class BoardAnimationController : MonoBehaviour
    {
        [SerializeField] private Transform floorSprite;
        [SerializeField] private Physics2DRaycaster raycaster;
        [SerializeField] private Transform maskGroup;

        private void Awake()
        {
            floorSprite.gameObject.SetActive(false);
            maskGroup.gameObject.SetActive(false);
            DOTween.SetTweensCapacity(500, 50);
            DOTween.Init();
            float ortho = CameraUtility.CalculateOrtho(5f);
            Camera.main.DOOrthoSize(ortho, 1f).SetDelay(0.25f);
        }


        public void AnimateBoardIn(NodeView startNode, Dictionary<Vector3Int, NodeView> nodes, Action onComplete = null)
        {
            StartCoroutine(BoardCreationSequence(startNode, nodes, onComplete));
        }

        public void AnimateBoardOut(NodeView startNode, Dictionary<Vector3Int, NodeView> nodes, Action onComplete = null)
        {
            StartCoroutine(DestroyBoardSequence(startNode, nodes, onComplete));
        }

        private IEnumerator BoardCreationSequence(NodeView startNode, Dictionary<Vector3Int, NodeView> nodes, Action onComplete)
        {
            foreach (var node in nodes.Values)
            {
                if (node.Index != Vector3Int.zero)
                {
                    node.gameObject.SetActive(false);
                }
            }

            raycaster.enabled = false;
            float cameraSize = CameraUtility.CalculateOrtho(1f);
            Camera.main.DOOrthoSize(cameraSize, 0.7f).SetEase(Ease.InQuad);

            yield return new WaitForSeconds(0.25f);

            floorSprite.gameObject.SetActive(true);
            floorSprite.DOScale(10, 0.5f).From(0).SetEase(Ease.InQuint);
            startNode.Animations.DoNodeHeight(-0.3f, 1.4f, Ease.Linear, true, true);

            yield return new WaitForSeconds(0.2f);

            maskGroup.gameObject.SetActive(true);
            maskGroup.DOMoveY(0f, 0.2f).From(-1f);
            Camera.main.DOShakeRotation(1f, 0.5f, 20, 90, false, ShakeRandomnessMode.Harmonic).SetDelay(0.2f).SetEase(Ease.OutExpo);

            yield return new WaitForSeconds(1f);

            foreach (var node in nodes.Values)
            {
                node.gameObject.SetActive(true);
            }
            floorSprite.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.5f);

            startNode.Animations.DoNodeHeight(1f, 0.2f, Ease.OutBack, true, true).easeOvershootOrAmplitude = 10f;
            NodeUtility.BroadSearch(-1, startNode, v => v.Neighbours, null, (n, i) => n.Pop(i));

            float maxX = (nodes.Values.Max(v => v.transform.position.x) + 1) * 2;
            float maxY = (nodes.Values.Max(v => v.transform.position.y) + 1) * 2;
            cameraSize = CameraUtility.CalculateOrtho(new Vector2(maxX, maxY));
            Camera.main.DOOrthoSize(cameraSize, 0.8f);

            yield return new WaitForSeconds(0.7f);

            maskGroup.gameObject.SetActive(false);
            raycaster.enabled = true;
        }

        private IEnumerator DestroyBoardSequence(NodeView startNode, Dictionary<Vector3Int, NodeView> nodes, Action onComplete)
        {
            raycaster.enabled = false;
            foreach (var node in nodes.Values)
            {
                if (node == startNode)
                {
                    continue;
                }
                node.Animations.DoEndPop().OnComplete(() =>
                {
                    Destroy(node.gameObject);
                });
            }


            yield return new WaitForSeconds(0.5f);
            float ortho = CameraUtility.CalculateOrtho(5f);
            Camera.main.DOOrthoSize(ortho, 1f);

            yield return new WaitForSeconds(1f);

            onComplete?.Invoke();
            raycaster.enabled = true;
        }

    }
}
